using Microsoft.AspNetCore.Mvc;
using Products.Api.Domain.Contracts;
using Products.Api.Domain.Interfaces;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        var result = _productsService.GetProducts();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(Guid id)
    {
        var result = _productsService.GetProduct(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductCreate product)
    {
        var result = await _productsService.CreateProductAsync(product);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductUpdate product)
    {
        var result = await _productsService.UpdateProductAsync(product);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _productsService.DeleteProductAsync(id);
        return Ok();
    }
}