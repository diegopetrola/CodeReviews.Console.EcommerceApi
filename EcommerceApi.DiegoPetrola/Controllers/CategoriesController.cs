using EcommerceApi.Models.DTOs;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CategoriesService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await service.GetCategories();
        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        var newDto = service.CreateCategory(dto);
        return CreatedAtAction(nameof(GetCategories), new { id = newDto.Id }, newDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        // TODO: implement result pattern
        await service.SoftDeleteCategory(id);
        return NoContent();
    }
}
