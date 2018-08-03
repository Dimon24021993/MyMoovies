using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace MyMovies.Parser
{
    public class AsyncProducerConsumerQueue<T> : IDisposable
    {
        public string Identity { get; set; }
        private readonly Action<T> m_consumer;
        private BlockingCollection<T> m_queue;
        private List<T> InProgress = new List<T>();
        private readonly CancellationTokenSource m_cancelTokenSrc;
        private List<Task> tasks;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AsyncProducerConsumerQueue(Action<T> consumer, string identity, int threadNumber = 1)
        {
            tasks = new List<Task>();
            if (consumer == null)
            {
                throw new ArgumentNullException(nameof(consumer));
            }

            Identity = identity;
            m_consumer = consumer;
            m_queue = new BlockingCollection<T>(new ConcurrentQueue<T>());
            m_cancelTokenSrc = new CancellationTokenSource();
            for (int i = 0; i < threadNumber; i++)
            {
                AddConsumer();
            }
        }

        public void AddConsumer()
        {
            var task = new Task(() => ConsumeLoop(m_cancelTokenSrc.Token));
            lock (tasks)
            {
                tasks.Add(task);
            }
            task.Start();
        }

        public void Produce(T value)
        {
            if (!m_queue.Contains(value) && !InProgress.Contains(value)) m_queue.Add(value);
        }
        public bool? IsAlive
        {
            get
            {
                bool? res = null;
                lock (tasks)
                {
                    res = tasks?.Any(x => x != null && !x.IsCompleted);
                }
                return res;
            }
        }



        public int? AliveCount
        {
            get
            {
                int? res = null;
                lock (tasks)
                {
                    res = tasks?.Count(x => !x?.IsCompleted ?? false);
                }

                return res;
            }
        }

        public int QueueCount => m_queue.Count;
        private void ConsumeLoop(CancellationToken cancelToken)
        {
            //logger.Info($"Thread start - {Identity}/{Thread.CurrentThread.ManagedThreadId}");
            while (!cancelToken.IsCancellationRequested)
            {
                try
                {
                    T item;
                    item = m_queue.Take(cancelToken);

                    if (item != null)
                    {
                        lock (InProgress) { InProgress.Add(item); }

                        m_consumer.Invoke(item);

                        lock (InProgress) { InProgress.Remove(item); }
                    }

                }
                catch (ArgumentNullException e)
                {
                    //logger.Info($"Thread stopped - {PrinterName}/{Thread.CurrentThread.ManagedThreadId} {e}");
                    break;
                }
                catch (OperationCanceledException e)
                {
                    //logger.Info($"Thread canceled - {PrinterName}/{Thread.CurrentThread.ManagedThreadId} {e.Message}");
                    return;
                }
                catch (ThreadAbortException e)
                {
                    //logger.Info($"Thread aborted - {PrinterName}/{Thread.CurrentThread.ManagedThreadId} {e.Message}");
                    break;
                }
                catch (UnauthorizedAccessException e)
                {
                    lock (m_cancelTokenSrc)
                    {
                        logger.Error(e);
                    }
                }
                catch (Exception e)
                {
                    lock (m_cancelTokenSrc)
                    {
                        logger.Error(e);
                    }
                }

            }
        }

        #region IDisposable

        private bool m_isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!m_isDisposed)
            {
                if (disposing)
                {
                    m_cancelTokenSrc.Cancel();
                    while (tasks.Any(x => !x.IsCompleted))
                    {
                        Thread.Sleep(1000);
                    }
                    m_queue.Dispose();
                }

                m_isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}