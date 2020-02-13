namespace ProductCatalog.Api.Messaging.Errors
{
    public interface IError
    {
        string Field { get; }
        string Message { get; }
    }
}