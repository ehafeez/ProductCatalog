using System.Collections.Generic;
using Newtonsoft.Json;
using ProductCatalog.Api.Messaging.Errors;

namespace ProductCatalog.Api.Messaging.Events
{
    public interface IProductDeletedEvent
    {
        bool State { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<IError> Errors { get; set; }
    }
}