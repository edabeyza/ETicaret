namespace ETicaret.Inventory.WebAPI.Models;
public sealed class InventoryItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
