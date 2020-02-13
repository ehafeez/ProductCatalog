using System;

namespace Services.ProductService.Core.Extensions
{
    public static class IModelExtensions
    {
        public static bool IsModeltNull(this IModel entity)
        {
            return entity == null;
        }

        public static bool IsModelEmpty(this IModel entity)
        {
            return entity.Id.Equals(Guid.Empty);
        }
    }
}