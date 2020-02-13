using System.Collections.Generic;
using System.Linq;
using Services.ProductService.App.Extensions;

namespace Services.ProductService.App.Utility
{
    public class PagingUtil
    {
        public static List<T> GetPaginatedResultAsync<T>(IQueryable<T> dataset, int? pageSize, int? currentPage,
            string sortBy, string sortDirection)
        {
            if (!string.IsNullOrEmpty(sortDirection) && !string.IsNullOrWhiteSpace(sortBy))
            {
                dataset = sortDirection.ToUpper() == "ASC"
                    ? dataset.OrderBy(sortBy)
                    : dataset.OrderByDescending(sortBy);
            }

            if (currentPage != null && pageSize.HasValue)
                dataset = dataset.Skip((currentPage.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            var items = dataset.ToList();
            return items;
        }
    }
}