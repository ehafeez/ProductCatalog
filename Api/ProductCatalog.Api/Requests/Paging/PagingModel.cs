namespace ProductCatalog.Api.Requests.Paging
{
    public class PagingModel
    {
        public int? PageSize { get; set; }
        public int? CurrentPage { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; } = "ASC";
    }
}