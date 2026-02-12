using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(SalesService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSale(int id)
    {
        var res = await service.GetSale(id);
        return MapToStatusCode(res);
    }

    [HttpGet("page/{page}")]
    public async Task<ActionResult<List<SaleDto>>> GetSalePage(int page)
    {
        var res = await service.GetSalesByPage(page);
        return MapToStatusCode(res);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale(CreateSaleDto dto)
    {
        var res = await service.CreateSale(dto);
        return MapToStatusCode(res);
    }

    private ActionResult MapToStatusCode<T>(Result<T> res)
    {
        if (res.IsSuccess)
            return Ok(res.Value);

        return res.Error.ErrorType switch
        {
            ErrorType.NotFound => NotFound(res.Error.Error),
            ErrorType.Invalid => BadRequest(res.Error.Error),
            _ => Problem(res.Error.Error)
        };
    }
}