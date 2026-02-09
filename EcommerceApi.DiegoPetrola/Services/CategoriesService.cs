using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class CategoriesService(EcommerceDbContext context)
{
    public async Task<IEnumerable<CategoryDto>> GetCategories()
    {
        var categories = await context.Categories
            .Where(c => !c.IsDeleted)
            .Select(c => new CategoryDto(c.Id, c.Name, c.IsDeleted))
            .ToListAsync();
        return categories;
    }

    public async Task SoftDeleteCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category is null) return;

        category.IsDeleted = true;
        await context.SaveChangesAsync();
        return;
    }

    public async Task<CategoryDto> CreateCategory(CreateCategoryDto dto)
    {
        var category = new Category { Name = dto.Name };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        return new CategoryDto(category.Id, category.Name, category.IsDeleted);
    }
}
