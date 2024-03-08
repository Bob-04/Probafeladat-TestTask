using AutoMapper;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;
using Products.Api.Domain.Models;

namespace Products.Api.Services;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _productsRepository;
    private readonly IMapper _mapper;

    public ProductsService(IProductsRepository productsRepository, IMapper mapper)
    {
        _productsRepository = productsRepository;
        _mapper = mapper;
    }

    public List<Product> GetProducts()
    {
        var products = _productsRepository.GetProducts();
        return products;
    }

    public Product GetProduct(Guid id)
    {
        var product = _productsRepository.GetProduct(id);
        if (product is null)
        {
            throw new ArgumentException("Product not exists");
        }

        return product;
    }

    public async Task<Product> CreateProductAsync(ProductCreate product)
    {
        var productModel = _mapper.Map<Product>(product);
        var createdProduct = await _productsRepository.CreateProductAsync(productModel);
        return createdProduct;
    }

    public async Task<Product> UpdateProductAsync(ProductUpdate product)
    {
        var productModel = _mapper.Map<Product>(product);
        ThrowIfProductNotExists(productModel.Id);

        var updatedProduct = await _productsRepository.UpdateProductAsync(productModel);
        return updatedProduct;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        ThrowIfProductNotExists(id);
        var isDeleted = await _productsRepository.DeleteProductAsync(id);
        return isDeleted;
    }

    private void ThrowIfProductNotExists(Guid id)
    {
        var isProductExists = _productsRepository.IsProductExists(id);
        if (!isProductExists)
        {
            throw new ArgumentException("Product not exists");
        }
    }
}