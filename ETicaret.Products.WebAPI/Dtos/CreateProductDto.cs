namespace ETicaret.Products.WebAPI.Dtos;

public sealed record CreateProductDto(
    string Name,
    decimal Price,
    int Stock);