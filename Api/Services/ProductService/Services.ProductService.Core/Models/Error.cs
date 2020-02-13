using ProductCatalog.Api.Messaging.Errors;

namespace Services.ProductService.Core.Models
{
    public class Error : IError
    {
        public string Field { get; }
        public string Message { get; }

        public Error(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}