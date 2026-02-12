using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class CategoriesService(EcommerceDbContext context)
{
    public async Task<Result<List<CategoryDto>>> GetCategories()
    {
        var categories = await context.Categories
            .Where(c => !c.IsDeleted)
            .Select(c => new CategoryDto(c.Id, c.Name, c.IsDeleted))
            .ToListAsync();

        return Result<List<CategoryDto>>.Ok(categories);
    }

    public async Task<Result<CategoryDto>> GetCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category is null || category.IsDeleted)
            return Result<CategoryDto>.NotFound("Category not found");
        var dto = new CategoryDto(category.Id, category.Name, category.IsDeleted);
        return Result<CategoryDto>.Ok(dto);
    }

    public async Task<Result<CategoryDto>> SoftDeleteCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category is null)
            return Result<CategoryDto>.NotFound("Record not found.");
        category.IsDeleted = true;
        await context.SaveChangesAsync();
        return Result<CategoryDto>.Ok(null!);
    }

    public async Task<Result<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        var category = new Category { Name = dto.Name };
        try
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }
        catch
        {
            return Result<CategoryDto>.Invalid("Duplicated category");
        }

        var newDto = new CategoryDto(category.Id, category.Name, category.IsDeleted);
        return Result<CategoryDto>.Ok(newDto);
    }
}
