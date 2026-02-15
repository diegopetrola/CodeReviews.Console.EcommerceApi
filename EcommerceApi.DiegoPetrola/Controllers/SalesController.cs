using EcommerceApi.Models.DTOs;
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
        return this.ToActionResult(res);
    }

    [HttpGet("page/{page}")]
    public async Task<ActionResult<List<SaleDto>>> GetSalePage(int page)
    {
        var res = await service.GetSalesByPage(page);
        return this.ToActionResult(res);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale(CreateSaleDto dto)
    {
        var res = await service.CreateSale(dto);
        return this.ToActionResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SaleDto>> DeleteSale(int id)
    {
        var res = await service.DeleteSale(id);
        return this.ToActionResult(res);
    }
}
