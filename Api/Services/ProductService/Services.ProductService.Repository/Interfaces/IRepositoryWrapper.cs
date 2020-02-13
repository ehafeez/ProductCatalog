namespace Services.ProductService.Repository.Interfaces
{
    public interface IRepositoryWrapper
    {
        IProductCatalogRepository Product { get; }
        void Save();
    }
}