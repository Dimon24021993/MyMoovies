using Microsoft.EntityFrameworkCore;
using MyMovies.BLL.Common;
using MyMovies.BLL.Exceptions;
using MyMovies.DAL;
using MyMovies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyMovies.BLL.Services
{
    public class EntitiesService : IDisposable
    {
        protected readonly DataBaseContext context;

        private bool _disposed;

        protected EntitiesService(DataBaseContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected async Task<T> GetByIdAsync<T>(Guid id, ICollection<Expression<Func<T, object>>> includes = null) where T : Entity
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.SingleOrDefaultAsync(entity => entity.Id == id);
        }

        protected async Task<T> GetSingleAsync<T>(ICollection<Expression<Func<T, bool>>> filters = null,
            ICollection<Expression<Func<T, object>>> includes = null) where T : Entity
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filters != null && filters.Any())
            {
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            }

            return await query.SingleOrDefaultAsync();
        }

        protected async Task<ICollection<T>> GetListAsync<T>(ICollection<Expression<Func<T, bool>>> filters = null,
            ICollection<Expression<Func<T, object>>> includes = null) where T : Entity
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filters != null && filters.Any())
            {
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            }

            return await query.ToListAsync();
        }

        protected async Task<bool> IsEntityExist<T>(T enity) where T : Entity
        {
            IQueryable<T> query = context.Set<T>();

            return await query.AsNoTracking().AnyAsync(x => x.Id == enity.Id);
        }

        protected async Task<PagedResults<T>> GetPagedResultsAsync<T>(
            int page,
            int pageSize,
            string orderBy,
            bool ascending,
            ICollection<Expression<Func<T, bool>>> filters = null,
            ICollection<Expression<Func<T, object>>> includes = null) where T : Entity
        {
            IQueryable<T> query = context.Set<T>();

            var skipAmount = pageSize * (page - 1);

            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filters != null && filters.Any())
            {
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            }

            var projection = query.OrderByPropertyOrField(orderBy, ascending)
                .Skip(skipAmount)
                .Take(pageSize);

            var totalNumberOfRecords = await query.CountAsync();
            var results = await projection.ToListAsync();

            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = totalNumberOfRecords / pageSize + (mod == 0 ? 0 : 1);

            return new PagedResults<T>
            {
                Results = results,
                PageNumber = page,
                PageSize = results.Count,
                TotalNumberOfPages = totalPageCount,
                TotalNumberOfRecords = totalNumberOfRecords,
            };
        }

        protected async Task Save<T>(T entity) where T : Entity
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    var dbEntity = await context.Set<T>().AsNoTracking().SingleOrDefaultAsync(x => x.Id == entity.Id);
                    if (dbEntity == null)
                    {
                        foreach (var prop in entity.GetType().GetProperties())
                        {
                            var elem = prop.GetValue(entity, null) as Entity;
                            if (elem != null)
                            {
                                context.Entry(elem).State = EntityState.Unchanged;
                            }
                        }
                        context.Set<T>().Add(entity);
                    }
                    else
                    {
                        context.Entry(entity).State = EntityState.Modified;
                    }

                    await context.SaveChangesAsync();
                    dbContextTransaction?.Commit();
                }
                catch (ValidationException ex)
                {
                    dbContextTransaction?.Rollback();
                    throw new InvalidModelException(ex.ValidationResult.ErrorMessage);
                }
                catch (Exception ex)
                {
                    dbContextTransaction?.Rollback();

                    throw new InvalidDbOperationException(ex.Message);
                }
            }
        }

        protected async Task Delete<T>(Guid id) where T : Entity
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    var entity = await context.Set<T>().FindAsync(id);
                    if (entity == null)
                    {
                        throw new InvalidInputParameterException();
                    }

                    context.Set<T>().Remove(entity);

                    await context.SaveChangesAsync();
                    dbContextTransaction?.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction?.Rollback();

                    throw new InvalidDbOperationException(ex.Message);
                }
            }
        }


        //public async Task ChangeEntities(ICollection<EntityWrapper> entities)
        //{
        //    using (var dbContextTransaction = context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            if (entities.Any())
        //            {
        //                foreach (var entity in entities)
        //                {
        //                    var type = entity.EntityObject;
        //                    var entityDbSet = context.Set(entity.EntityObject.GetType());
        //                    if (entityDbSet != null)
        //                    {
        //                        switch (entity.Operation)
        //                        {
        //                            case CrudOperation.Create:
        //                            case CrudOperation.Update:
        //                                var addEntity = await entityDbSet.FindAsync(entity.EntityObject.Id);
        //                                if (addEntity == null)
        //                                {
        //                                    entityDbSet.Add(entity.EntityObject);
        //                                }
        //                                else
        //                                {
        //                                    entityDbSet.Attach(entity.EntityObject);
        //                                }
        //                                break;

        //                            case CrudOperation.Delete:
        //                                var entitie = await entityDbSet.FindAsync(entity.EntityObject.Id);
        //                                if (entitie != null)
        //                                {
        //                                    entityDbSet.Attach(entitie);
        //                                    entityDbSet.Remove(entitie);
        //                                }
        //                                break;

        //                            default:
        //                                throw new ArgumentOutOfRangeException();
        //                        }
        //                    }
        //                }

        //                await context.SaveChangesAsync();
        //                dbContextTransaction?.Commit();
        //            }
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            dbContextTransaction?.Rollback();

        //            var error = ex.EntityValidationErrors.First().ValidationErrors.First();
        //            throw new InvalidModelException(error.ErrorMessage);
        //        }
        //        catch (Exception ex)
        //        {
        //            dbContextTransaction?.Rollback();

        //            throw new InvalidDbOperationException(ex.Message);
        //        }
        //    }
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                context?.Dispose();
            }

            _disposed = true;
        }

        ~EntitiesService()
        {
            Dispose(false);
        }
    }
}
