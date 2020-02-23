using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductCatalog.Api.Messaging.Commands;

namespace Services.ProductService.Core.Models
{
    [Table("Product")]
    public class Product : IModel
    {
        [Key] [Column] public Guid Id { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(100, ErrorMessage = "Code can't be longer than 100 characters")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(250, ErrorMessage = "Name can't be longer than 250 characters")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "Description can't be longer than 300 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Blob Name is required")]
        public string BlobName { get; set; }

        [Required(ErrorMessage = "File extension is required")]
        public string PhotoName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }

        [NotMapped] public List<Error> Errors { get; private set; } = new List<Error>();
        [ConcurrencyCheck] public byte[] RowVersion { get; set; } //Optimistic concurrency

        public static Product CreateProduct(ICreateProductCommand command)
        {
            var product = new Product
            {
                Code = command.Code,
                Name = command.Name,
                Description = command.Description,
                PhotoName = command.PhotoName,
                BlobName = command.BlobName,
                Price = command.Price,
                LastUpdated = DateTime.Now
            };

            product.ValidateModel();
            return product;
        }

        public static Product CreateProduct(IUpdateProductCommand command)
        {
            if (command == null) return null;

            var product = new Product
            {
                Id = command.Id,
                Code = command.Code,
                Name = command.Name,
                Description = command.Description,
                PhotoName = command.PhotoName,
                BlobName = command.BlobName,
                Price = command.Price,
                LastUpdated = DateTime.Now
            };

            product.ValidateModel();
            return product;
        }

        private void ValidateModel()
        {
            Errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(Code))
                Errors.Add(new Error("Code", "Field must not be empty"));

            if (string.IsNullOrWhiteSpace(Name))
                Errors.Add(new Error("Name", "Field must not be empty"));

            //if (string.IsNullOrWhiteSpace(BlobName))
            //    Errors.Add(new Error("BlobName", "Field must not be empty"));

            if (string.IsNullOrWhiteSpace(PhotoName))
                Errors.Add(new Error("Photo Name", "Field must not be empty"));

            if (Price < 0.0)
                Errors.Add(new Error("Price", "Field must not be less than 0"));
        }
    }
}