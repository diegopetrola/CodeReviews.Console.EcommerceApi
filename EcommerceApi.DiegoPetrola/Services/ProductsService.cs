using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class ProductsService(EcommerceDbContext context)
{
    public async Task<Result<List<ProductDto>>> GetProducts(int page)
    {
        var products = await context.Products
            .Skip(20 * page)
            .Take(20)
            .Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Category.Id, p.Category.Name))
            .ToListAsync();

        return Result<List<ProductDto>>.Ok(products);
    }

    public async Task<Result<ProductDto>> GetProduct(int id)
    {
        var product = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return Result<ProductDto>.NotFound("Product not found");

        var dto = new ProductDto(product.Id, product.Name, product.Price, product.Category.Id, product.Category.Name);
        return Result<ProductDto>.Ok(dto);
    }

    public async Task<Result<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        try
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Result<ProductDto>.Invalid("Invalid product data or duplicate entry");
        }

        var newDto = new ProductDto(product.Id, product.Name, product.Price, product.CategoryId, product.Category.Name);
        return Result<ProductDto>.Ok(newDto);
    }

    public async Task<Result<ProductDto>> SoftDeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
            return Result<ProductDto>.NotFound("Product not found");

        product.IsDeleted = true;
        await context.SaveChangesAsync();
        return Result<ProductDto>.Ok(null);
    }
}
