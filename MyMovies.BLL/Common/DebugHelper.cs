using System;
using System.Collections.Generic;
using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace MyMovies.BLL.Common
{
    public static class DebugHelper
    {
        public static List<string> GetAllScheduledJobs()
        {
            var shedulersInfo = new List<string>();

            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            var jobGroups = scheduler.GetJobGroupNames();
            foreach (var group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = scheduler.GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    var detail = scheduler.GetJobDetail(jobKey);
                    var triggers = scheduler.GetTriggersOfJob(jobKey);
                    foreach (ITrigger trigger in triggers)
                    {
                        var jobInfo =
                            $"Group: {group}, Job Key: {jobKey.Name}, Trigger Name: {trigger.GetType().Name}," +
                            $"Desc: {detail.Description},Key Name: {trigger.Key.Name}, Key Group: {trigger.Key.Group}" +
                            $"State: {scheduler.GetTriggerState(trigger.Key)}";
                                      
                        DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            jobInfo += $"Next Fire Time: {nextFireTime.Value.LocalDateTime}";
                        }

                        DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            jobInfo += $"Previous Fire Time: {previousFireTime.Value.LocalDateTime}";
                        }

                        shedulersInfo.Add(jobInfo);
                    }
                }
            }

            return shedulersInfo;
        }
    }
}
