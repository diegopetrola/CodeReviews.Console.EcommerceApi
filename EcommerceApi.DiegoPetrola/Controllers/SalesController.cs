using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(EcommerceDbContext context) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSale(int id)
    {
        var sale = await context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale is null) return NotFound();

        var dto = new SaleDto(
            sale.Id,
            sale.SaleDate,
            [.. sale.SaleItems.Select(si => new SaleItemDto(
                si.ProductId,
                si.Product.Name,
                si.Quantity,
                si.Product.Price))],
            sale.SaleItems.Sum(si => si.Quantity * si.Product.Price)
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale(CreateSaleDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            return BadRequest("Sale must contain at least one item.");

        var sale = new Sale { SaleDate = DateTime.UtcNow };

        foreach (var item in dto.Items)
        {
            var product = await context.Products.FindAsync(item.ProductId);
            if (product == null) return BadRequest($"Product {item.ProductId} not found.");

            sale.SaleItems.Add(new SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        context.Sales.Add(sale);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale.Id);
    }
}