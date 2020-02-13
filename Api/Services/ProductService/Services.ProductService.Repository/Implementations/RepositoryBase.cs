using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Services.ProductService.Data;
using Services.ProductService.Repository.Interfaces;

namespace Services.ProductService.Repository.Implementations
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ProductDbContext ProductDbContext { get; set; }

        public RepositoryBase(ProductDbContext productDbContext)
        {
            ProductDbContext = productDbContext;
        }

        public IQueryable<T> GetAll()
        {
            //return ProductDbContext.Set<T>().AsNoTracking();
            return ProductDbContext.Set<T>();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            //return ProductDbContext.Set<T>().AsNoTracking().Where(expression);
            return ProductDbContext.Set<T>().Where(expression);
        }

        public void Create(T entity)
        {
            ProductDbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            ProductDbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            ProductDbContext.Set<T>().Remove(entity);
        }
    }
}