using Microsoft.AspNetCore.Mvc;
using MultiTenancy.Dtos;

namespace MultiTenancy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreatedAsync(CreateProductDto dto)
    {
        Product product = new ()
        {
            Name = dto.Name,
            Description = dto.Description,
            Rate = dto.Rate,
        };

        var createdProduct = await _productService.CreatedAsync(product);

        return Ok(createdProduct);
    }
}