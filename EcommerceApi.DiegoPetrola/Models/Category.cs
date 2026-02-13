using EcommerceApi.Models.DTOs;

namespace EcommerceApi.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public ICollection<Product> Products { get; set; } = [];
    public CategoryDto ToDto()
    {
        return new CategoryDto(Id, Name, IsDeleted);
    }
}
