using System.Collections.Generic;
using Newtonsoft.Json;
using ProductCatalog.Api.Messaging.Errors;

namespace ProductCatalog.Api.Messaging.Events
{
    public interface IPhotoUploaded
    {
        string TimeStamp { get; set; }
        bool IsUploaded { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string BlobName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<IError> Errors { get; set; }
    }
}