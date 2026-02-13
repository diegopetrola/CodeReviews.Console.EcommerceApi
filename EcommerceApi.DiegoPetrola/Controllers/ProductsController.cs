using EcommerceApi.Models.DTOs;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductsService service) : ControllerBase
{
    [HttpGet("page/{page}")]
    public async Task<ActionResult<List<ProductDto>>> GetProducts(int page)
    {
        var res = await service.GetProducts(page);
        return this.ToActionResult(res);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var res = await service.GetProduct(id);
        return this.ToActionResult(res);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var res = await service.CreateProduct(dto);
        return this.ToActionResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductDto>> DeleteProduct(int id)
    {
        var res = await service.SoftDeleteProduct(id);
        return this.ToActionResult(res);
    }
}
