using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;
using Products.Api.Domain.Models;
using Products.Api.Services;

namespace Products.UnitTests;

public class ProductServiceTests
{
    private readonly IProductsRepository  _productsRepository;
    private readonly IMapper _mapper; 
    private readonly ProductsService _productsService;
    public ProductServiceTests()
    {
            _productsRepository = Substitute.For<IProductsRepository>();
            _mapper = Substitute.For<IMapper>();
            _productsService = new ProductsService(_productsRepository, _mapper);
    }
    
    [Fact]
    public void GetProducts_ShouldReturnListOfProducts()
    {
        // Arrange
        List<Product> products = new()
        {
            new() { Id = Guid.NewGuid(), Name = "Product1", Price = 100 },
            new() { Id = Guid.NewGuid(), Name = "Product2", Price = 100 }
        };
        _productsRepository.GetProducts()!
            .Returns(products);
        
        // Act
        List<Product> result = _productsService.GetProducts();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(products);     
    }
    
    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product {Id = Guid.NewGuid(), Name = "Product1", Price = 100};
        _productsRepository.GetProduct(Arg.Any<Guid>())!
            .Returns(c => product);
        
        // Act
        var result =  _productsService.GetProduct(product.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }
    
    [Fact]
    public void GetProductAsync_WhenProductNotExists_ShouldReturnNull()
    {
        // Arrange
        _productsRepository.GetProduct(Arg.Any<Guid>())!
            .Returns(c => null!);
        
        // Act
        Func<Product> act = () =>  _productsService.GetProduct(Guid.NewGuid());
        
        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Product not exists");
    }
    
    [Fact]
    public async Task CreateProductAsync_ShouldReturnCreatedProduct()
    {
        // Arrange
        var productCreate = new ProductCreate { Name = "Product1", Price = 100 };
        var product = new Product {Id = Guid.NewGuid(), Name = "Product1", Price = 100};
        _mapper.Map<Product>(productCreate)!
            .Returns(product);
        _productsRepository.CreateProductAsync(product)!
            .Returns(Task.FromResult(product));
        
        // Act
        var result = await _productsService.CreateProductAsync(productCreate);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }
    
    [Fact]
    public async Task UpdateProductAsync_ShouldReturnUpdatedProduct()
    {
        // Arrange
        var productUpdate = new ProductUpdate { Id = Guid.NewGuid(), Name = "Product1", Price = 100 };
        var product = new Product {Id = productUpdate.Id, Name = "Product1", Price = 100};
        _mapper.Map<Product>(productUpdate)!
            .Returns(product);
        _productsRepository.IsProductExists(Arg.Any<Guid>())!
            .Returns(true);
        _productsRepository.UpdateProductAsync(product)!
            .Returns(Task.FromResult(product));
        
        // Act
        var result = await _productsService.UpdateProductAsync(productUpdate);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(product);
    }
    
    [Fact]
    public async Task UpdateProductAsync_WhenProductNotExists_ShouldThrowException()
    {
        // Arrange
        var productUpdate = new ProductUpdate { Id = Guid.NewGuid(), Name = "Product1", Price = 100 };
        _mapper.Map<Product>(productUpdate)!
            .Returns(new Product {Id = productUpdate.Id, Name = "Product1", Price = 100});
        _productsRepository.IsProductExists(Arg.Any<Guid>())!
            .Returns(false);
        
        // Act
        Func<Task> act = async () => await _productsService.UpdateProductAsync(productUpdate);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Product not exists");
    }
    
    [Fact]
    public async Task DeleteProductAsync_ShouldReturnTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        _productsRepository.IsProductExists(id)!
            .Returns(true);
        _productsRepository.DeleteProductAsync(id)!
            .Returns(Task.FromResult(true));
        
        // Act
        var result = await _productsService.DeleteProductAsync(id);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteProductAsync_WhenProductNotExists_ShouldReturnFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        _productsRepository.IsProductExists(Arg.Any<Guid>())!
            .Returns(false);
        
        // Act
        Func<Task> act = async () => await _productsService.DeleteProductAsync(id);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Product not exists");
    }
}