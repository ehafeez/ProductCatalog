using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalog.Api.Messaging.Commands;
using Services.ProductService.App.Dtos;
using Services.ProductService.App.Interfaces;
using Services.ProductService.App.Utility;
using Services.ProductService.Core.Models;
using Services.ProductService.Repository.Interfaces;

namespace Services.ProductService.App.Implementations
{
    public class ProductCatalogService : IProductCatalog
    {
        private readonly IRepositoryWrapper _repository;

        public ProductCatalogService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="productCommand"></param>
        /// <returns></returns>
        public async Task<CreateProductDto> CreateProduct(ICreateProductCommand productCommand)
        {
            CreateProductDto response = null;
            try
            {
                var product = Product.CreateProduct(productCommand);

                if (product == null)
                    return await Task.Run(() => response = new CreateProductDto(null));

                if (product.Errors.Count > 0)
                    return await Task.Run(() => response = new CreateProductDto(product));

                var result = _repository.Product.GetByCondition(x => x.Code.Equals(product.Code) || x.Name.Equals(product.Name));
                if (result?.Count() == 0)
                {
                    _repository.Product.CreateProduct(product);
                    _repository.Save();
                    response = new CreateProductDto(product);
                }
                else
                {
                    response = new CreateProductDto(product)
                    {
                        Errors = new List<Error>
                        {
                            new Error("Id", "Product already exists")
                        }
                    };
                }

                return await Task.Run(() => response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                response = CreateProductDto.PrepareExceptionResponse(null, "ERROR_SYSTEM");
                return await Task.Run(() => response);
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="productCommand"></param>
        /// <returns></returns>
        public async Task<UpdateProductDto> UpdateProduct(IUpdateProductCommand productCommand)
        {
            UpdateProductDto response = null;
            try
            {
                var product = Product.CreateProduct(productCommand);

                if (product == null)
                    return await Task.Run(() => response = new UpdateProductDto(null));

                if (product.Errors.Count > 0)
                    return await Task.Run(() => response = new UpdateProductDto(product));

                var existingProduct = _repository.Product.GetProductById(product.Id);
                if (existingProduct != null)
                {
                    var result = _repository.Product.GetByCondition(x =>
                        (x.Code.Equals(product.Code) || x.Name.Equals(product.Name)) && x.Id != product.Id);

                    if (result?.Count() == 0)
                    {
                        _repository.Product.UpdateProduct(existingProduct, product);
                        _repository.Save();

                        response = new UpdateProductDto(product);
                    }
                }
                else
                {
                    response = new UpdateProductDto(product)
                    {
                        Errors = new List<Error>
                        {
                            new Error("Product", "Product doesn't exists")
                        }
                    };
                }

                return await Task.Run(() => response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                response = UpdateProductDto.PrepareExceptionResponse(null, "ERROR_SYSTEM");
                return await Task.Run(() => response);
            }
        }

        /// <summary>
        /// Delete the product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<DeleteProductDto> DeleteProduct(string productId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productId))
                {
                    return await Task.Run(() => new DeleteProductDto(false)
                    {
                        Errors = new List<Error>
                        {
                            new Error("productId", "Field must not be empty")
                        }
                    });
                }

                var product = _repository.Product.GetProductById(Guid.Parse(productId));
                if (product != null)
                {
                    _repository.Product.DeleteProduct(product);
                    _repository.Save();
                    return await Task.Run(() => new DeleteProductDto(true));
                }

                return new DeleteProductDto(false)
                {
                    Errors = new List<Error>
                    {
                        new Error("Product", "Product doesn't exists")
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.Run(() => DeleteProductDto.PrepareExceptionResponse(null, "ERROR_SYSTEM"));
            }
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<GetProductDto> GetProduct(string productId)
        {
            GetProductDto response = null;
            try
            {
                var product = _repository.Product.GetProductById(Guid.Parse(productId));
                if (product != null)
                {
                    response = new GetProductDto(product.Id, product.Code, product.Name, product.Description,
                        null, product.PhotoName, product.Price, product.LastUpdated.ToString(), product.BlobName);

                    return await Task.Run(() => response);
                }

                return await Task.Run(() => response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.Run(() => GetProductDto.PrepareExceptionResponse(Guid.Empty, "ERROR_SYSTEM"));
            }
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        public async Task<List<GetProductDto>> GetAllProducts(IGetAllProductsCommand productCommand)
        {
            var response = new List<GetProductDto>();
            try
            {
                var result = _repository.Product.GetAllProducts();
                result?.ForEach(prod =>
                {
                    response.Add(new GetProductDto(prod.Id, prod.Code, prod.Name, prod.Description, null,
                        prod.PhotoName, prod.Price, prod.LastUpdated.ToString(), prod.BlobName));
                });

                if (productCommand?.CurrentPage != null && productCommand.PageSize.HasValue)
                {
                    var dataset = response.AsQueryable();
                    response = PagingUtil.GetPaginatedResultAsync(dataset, productCommand.PageSize,
                        productCommand.CurrentPage, productCommand.SortBy, productCommand.SortDirection);

                    return await Task.Run(() => response);
                }

                return await Task.Run(() => response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.Run(() => response);
            }
        }
    }
}