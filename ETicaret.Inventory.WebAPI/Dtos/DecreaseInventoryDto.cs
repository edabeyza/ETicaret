namespace ETicaret.Inventory.WebAPI.Dtos;
public sealed class DecreaseInventoryDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
