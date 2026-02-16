namespace EcommerceApi.Models;

public class Sale
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<SaleItem> SaleItems { get; set; } = [];
}
