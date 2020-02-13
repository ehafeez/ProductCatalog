using System;
using System.Text;

namespace Services.ProductService.App.Infrastructure
{
    public static class ErrorDescription
    {
        public static string Create(Exception exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine(exception.Message);
            if (exception.InnerException != null)
            {
                sb.AppendLine(exception.InnerException.Message);
            }

            sb.AppendLine(exception.StackTrace);
            return sb.ToString();
        }
    }
}