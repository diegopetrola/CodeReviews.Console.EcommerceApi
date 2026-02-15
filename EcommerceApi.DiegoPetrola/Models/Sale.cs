using EcommerceApi.Models.DTOs;

namespace EcommerceApi.Models;

public class Sale
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<SaleItem> SaleItems { get; set; } = [];
    public SaleDto ToDto()
    {
        return new SaleDto(
            Id,
            SaleDate,
            [.. SaleItems.Select(si => si.ToDto())],
            SaleItems.Sum(si => si.Quantity * si.ProductPrice)
        );
    }
}
