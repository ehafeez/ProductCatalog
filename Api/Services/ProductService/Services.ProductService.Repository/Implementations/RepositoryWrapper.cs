using Services.ProductService.Data;
using Services.ProductService.Repository.Interfaces;

namespace Services.ProductService.Repository.Implementations
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ProductDbContext _dbContext;
        private IProductCatalogRepository _product;
        public IProductCatalogRepository Product => _product ?? (_product = new ProductCatalogRepository(_dbContext));

        public RepositoryWrapper(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}