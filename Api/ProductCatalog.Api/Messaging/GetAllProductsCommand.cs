using ProductCatalog.Api.Messaging.Commands;

namespace ProductCatalog.Api.Messaging
{
    public class GetAllProductsCommand : IGetAllProductsCommand
    {
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }

        public GetAllProductsCommand(int? pageSize, int? currentPage, string sortBy, string sortDirection)
        {
            PageSize = pageSize;
            CurrentPage = currentPage;
            SortBy = sortBy;
            SortDirection = sortDirection;
        }
    }
}