using Products.Api.Domain.Models;

namespace Products.Api.Domain.Interfaces;

public interface IProductsRepository
{
    List<Product> GetProducts();
    Product? GetProduct(Guid id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid id);
    bool IsProductExists(Guid id);
}