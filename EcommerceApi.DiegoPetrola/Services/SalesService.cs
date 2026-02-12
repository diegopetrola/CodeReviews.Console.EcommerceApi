using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class SalesService(EcommerceDbContext context)
{
    private static SaleDto GetSaleDto(Sale sale)
    {
        return new SaleDto(
            sale.Id,
            sale.SaleDate,
            [.. sale.SaleItems.Select(si => new SaleItemDto(
                si.ProductId,
                si.Product.Name,
                si.Quantity,
                si.Product.Price))],
            sale.SaleItems.Sum(si => si.Quantity * si.Product.Price)
        );
    }

    public async Task<Result<List<SaleDto>>> GetSalesByPage(int page)
    {
        var sales = await context.Sales
            .Skip(page * 20)
            .Take(20)
            .Select(s => GetSaleDto(s))
            .ToListAsync();

        return Result<List<SaleDto>>.Ok(sales);
    }

    public async Task<Result<SaleDto>> GetSale(int id)
    {
        var sale = await context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale is null) return Result<SaleDto>.NotFound("Sale not found");

        var dto = GetSaleDto(sale);
        return Result<SaleDto>.Ok(dto);
    }

    public async Task<Result<SaleDto>> CreateSale(CreateSaleDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            return Result<SaleDto>.Invalid("Sale must contain at least one item.");

        var sale = new Sale { SaleDate = DateTime.UtcNow };

        foreach (var item in dto.Items)
        {
            var product = await context.Products.FindAsync(item.ProductId);
            if (product == null) return Result<SaleDto>.Invalid($"Can't make sale of invalid product {item.ProductId}!");

            sale.SaleItems.Add(new SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        context.Sales.Add(sale);
        await context.SaveChangesAsync();

        return Result<SaleDto>.Ok(GetSaleDto(sale));
    }
}
