namespace ETicaret.Products.WebAPI.Dtos;
public sealed record ChangeProductStockDto(
    Guid ProductId,
    int Quantity
    );
