using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Requests.Paging;
using MassTransit;
using Microsoft.Extensions.Options;
using ProductCatalog.Api.Infrastructure.Settings;
using ProductCatalog.Api.Messaging;
using ProductCatalog.Api.Messaging.Commands;
using ProductCatalog.Api.Messaging.Events;
using ProductCatalog.Api.Requests.ProductCatalog;

namespace ProductCatalog.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    public class ProductController : Controller
    {
        private readonly IBus _bus;
        private readonly Uri _serviceAddress;
        private readonly string[] _acceptedFileTypes = { "jpg", "jpeg", "png" };

        public ProductController(IOptions<ServiceBusQueuesSettings> serviceBusQueueOptions, IBus bus)
        {
            _bus = bus;
            var hostName = @"sb://" + _bus.Address.Host;
            var hostUri = new Uri(hostName);
            var serviceBusQueuesSettings = serviceBusQueueOptions.Value;
            _serviceAddress = new Uri(hostUri + serviceBusQueuesSettings.ProductServiceQueue);
        }

        [HttpGet, ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProducts([FromQuery] PagingModel paging)
        {
            try
            {
                var client = _bus.CreateRequestClient<IGetAllProductsCommand, IAllProductRetrievedEvent>(
                    _serviceAddress, TimeSpan.FromSeconds(60));
                var getProductsCommand = new GetAllProductsCommand(paging.PageSize, paging.CurrentPage, paging.SortBy,
                    paging.SortDirection);

                var response = await client.Request(getProductsCommand);
                if (paging.CurrentPage != null && paging.PageSize.HasValue)
                {
                    var totalCount = response.Products?.Count ?? 0;
                    var totalPages = (int) Math.Ceiling(totalCount / (double) paging.PageSize);
                    return Ok(new {TotalCount = totalCount, totalPages, ds = response.Products});
                }

                return Ok(response?.Products);
            }
            catch (Exception e)
            {
                return BadRequest($"Can't get the products: {e.Message}");
            }
        }

        [HttpGet, ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductById([FromQuery(Name = "productId")] string productId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productId))
                    throw new Exception("Product Id is not valid");

                var client = _bus.CreateRequestClient<IGetProductCommand, IProductRetrievedEvent>(_serviceAddress,
                        TimeSpan.FromSeconds(20));
                var getCompanyCommand = new GetProductCommand(productId);
                var response = await client.Request(getCompanyCommand);
                return Ok(response?.ProductDto);
            }
            catch (Exception e)
            {
                return BadRequest($"Can't get the product: {e.Message}");
            }
        }

        [HttpPost, ProducesResponseType(StatusCodes.Status200OK),
         ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));

                if (request == null)
                    throw new Exception("Product model is null");

                if (request.Photo.Length > 1 * 1024 * 1024)
                    return BadRequest("Max file size exceeded. Only 1 MB allowed");

                if (_acceptedFileTypes.All(e => e != request.PhotoName.Split('.')[1].ToLower()))
                    return BadRequest("Invalid file type");

                var client = _bus.CreateRequestClient<ICreateProductCommand, IProductCreatedEvent>(_serviceAddress, TimeSpan.FromSeconds(20));
                var createProductCommand = new CreateProductCommand(request);
                var response = await client.Request(createProductCommand);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest($"Can't create the product: {e.Message}");
            }
        }

        [HttpPut("{id}"), ProducesResponseType(StatusCodes.Status200OK),
         ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status409Conflict),
         ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductRequest updateRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("Id is not valid");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                if (updateRequest.Photo.Length > 1 * 1024 * 1024)
                    return BadRequest("Max file size exceeded. Only 1MB allowed");

                if (_acceptedFileTypes.All(e => e != updateRequest.PhotoName.Split('.')[1].ToLower()))
                    return BadRequest("Invalid file type");

                var client = _bus.CreateRequestClient<IUpdateProductCommand, IProductUpdatedEvent>(_serviceAddress,
                    TimeSpan.FromSeconds(20));
                var updateCompanyCommand = new UpdateProductCommand(id, updateRequest);
                var response = await client.Request(updateCompanyCommand);
                return Ok(response);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex);
            }
            catch (Exception e)
            {
                return BadRequest($"Update product exception: {e.Message}");
            }
        }

        [HttpDelete("{id}"), ProducesResponseType(StatusCodes.Status200OK),
         ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status400BadRequest),
         ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    throw new Exception("Product Id is not valid");

                var client = _bus.CreateRequestClient<IDeleteProductCommand, IProductDeletedEvent>(_serviceAddress,
                        TimeSpan.FromSeconds(20));
                var deleteCompanyCommand = new DeleteProductCommand(id);
                var response = await client.Request(deleteCompanyCommand);
                return Ok(response);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex);
            }
            catch (Exception e)
            {
                return BadRequest($"Delete product exception: {e.Message}");
            }
        }
    }
}