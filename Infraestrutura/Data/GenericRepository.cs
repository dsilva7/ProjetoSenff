using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infraestrutura.Data
{
    public class GenericRepository<C, T> where C : DbContext where T : class
    {
        protected C dbContext;
        protected DbSet<T> dbSet;

        public GenericRepository(C dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> AsQueryable()
        {
            return this.dbSet;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return this.dbSet.Where(where);
        }

        public T First(Expression<Func<T, bool>> where)
        {
            return this.dbSet.First(where);
        }

        public Task<T> FirstAsync(Expression<Func<T, bool>> where)
        {
            return this.dbSet.FirstAsync(where);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.Any(predicate);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return this.dbSet.AnyAsync(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return this.dbSet.FirstOrDefault(where);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where)
        {
            return this.dbSet.FirstOrDefaultAsync(where);
        }

        public void Remove(T entity)
        {
            this.dbSet.Remove(entity);
        }

        public void Add(T entity)
        {
            this.dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            this.dbSet.Update(entity);
        }

        public void Attach(T entity)
        {
            this.dbSet.Attach(entity);
        }

        public void LoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
        {
            this.dbContext
                .Entry(entity)
                .Collection<TProperty>(propertyExpression)
                .Load();

        }

        public void LoadReference<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class
        {
            this.dbContext
                .Entry(entity)
                .Reference<TProperty>(propertyExpression)
                .Load();

        }
    }
}
