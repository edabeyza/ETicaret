namespace ETicaret.Inventory.WebAPI.Dtos;
public sealed class CreateInventoryDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}