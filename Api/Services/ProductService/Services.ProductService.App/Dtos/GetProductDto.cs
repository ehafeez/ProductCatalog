using System;
using System.Collections.Generic;
using ProductCatalog.Api.Messaging.Interfaces;
using Services.ProductService.Core.Models;

namespace Services.ProductService.App.Dtos
{
    public class GetProductDto : IProductDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public string BlobName { get; set; }
        public string PhotoName { get; set; }
        public double Price { get; set; }
        public string LastUpdated { get; set; }
        public List<Error> Errors { get; set; }

        public GetProductDto(Guid id, string code, string name, string description, byte[] photo, string photoName,
            double price, string lastUpdated, string blobName)
        {
            if (id != Guid.Empty)
            {
                Id = id;
                Code = code;
                Name = name;
                Description = description;
                Photo = photo;
                PhotoName = photoName;
                Price = price;
                BlobName = blobName;
                LastUpdated = lastUpdated;
            }
        }

        internal static GetProductDto PrepareExceptionResponse(Guid id, string exception)
        {
            return new GetProductDto(id, "", "", "", null, "", 0, "", "")
            {
                Errors = new List<Error>
                {
                    new Error("Exception", exception)
                }
            };
        }
    }
}