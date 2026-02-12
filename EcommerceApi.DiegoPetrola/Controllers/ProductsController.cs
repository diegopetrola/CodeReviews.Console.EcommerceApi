using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
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
        return Ok(res.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var res = await service.GetProduct(id);
        if (!res.IsSuccess)
            return res.Error.ErrorType switch
            {
                (ErrorType.NotFound) => NotFound(res.Error.Error),
                _ => Problem(res.Error.Error),
            };

        return Ok(res.Value);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var res = await service.CreateProduct(dto);
        if (!res.IsSuccess)
            return BadRequest(res.Error.Error);
        return Ok(res.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductDto>> DeleteProduct(int id)
    {
        var res = await service.SoftDeleteProduct(id);
        if (!res.IsSuccess)
            return NotFound(res.Error.Error);
        return Ok(res.Value);
    }
}
