using Products.Api.Domain.Contracts;
using Products.Api.Domain.Models;

namespace Products.Api.Domain.Interfaces;

public interface IProductsService
{
    List<Product> GetProducts();
    Product GetProduct(Guid id);
    Task<Product> CreateProductAsync(ProductCreate product);
    Task<Product> UpdateProductAsync(ProductUpdate product);
    Task<bool> DeleteProductAsync(Guid id);
}