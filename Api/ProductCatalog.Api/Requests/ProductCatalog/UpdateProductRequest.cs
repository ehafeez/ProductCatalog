using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.Requests.ProductCatalog
{
    public class UpdateProductRequest
    {
        [Required(ErrorMessage = "Code is required")]
        [StringLength(100, ErrorMessage = "Code can't be longer than 100 characters")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(250, ErrorMessage = "Name can't be longer than 250 characters")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "Description can't be longer than 300 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        public string Photo { get; set; }

        [Required(ErrorMessage = "Photo Name is required")]
        public string PhotoName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }
    }
}