namespace ProductCatalog.Api.Messaging.Commands
{
    public interface IGetAllProductsCommand
    {
        int? PageSize { get; set; }
        int? CurrentPage { get; set; }
        string SortBy { get; set; }
        string SortDirection { get; set; }
    }
}