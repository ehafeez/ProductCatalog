using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ProductCatalog.Api.Messaging.Errors;

namespace ProductCatalog.Api.Messaging.Events
{
    public interface IProductCreatedEvent
    {
        Guid Id { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<IError> Errors { get; set; }
    }
}