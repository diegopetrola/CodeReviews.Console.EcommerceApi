using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
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
        var res = await service.GetCategories();
        if (!res.IsSuccess)
            return Problem(res.Error.Error);
        return Ok(res.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var res = await service.GetCategory(id);
        if (!res.IsSuccess)
            switch (res.Error.ErrorType)
            {
                case (ErrorType.NotFound):
                    return NotFound(res.Error.Error);
                default:
                    return Problem(res.Error.Error);
            }

        return Ok(res.Value);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        var res = await service.CreateCategory(dto);
        if (!res.IsSuccess)
            return BadRequest(res.Error.Error);

        return Ok(res.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var res = await service.SoftDeleteCategory(id);
        if (!res.IsSuccess)
            return Problem(res.Error.Error);

        return NoContent();
    }
}
