using ETicaret.Inventory.WebAPI.Dtos;
using ETicaret.Inventory.WebAPI.Models;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

List<InventoryItem> stocks = new();

app.MapGet("/getall", () =>
{
    List<InventoryDto> response = stocks
        .Select(s => new InventoryDto
        {
            ProductId = s.ProductId,
            Quantity = s.Quantity
        }).ToList();

    return Results.Ok(new Result<List<InventoryDto>>(response));
});

app.MapPost("/create", (CreateInventoryDto request) =>
{
    var existing = stocks.FirstOrDefault(s => s.ProductId == request.ProductId);

    if (existing is null)
    {
        stocks.Add(new InventoryItem
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity
        });
    }
    else
    {
        existing.Quantity = request.Quantity;
    }

    return Results.Ok(new Result<string>("Stok baþarýyla kaydedildi"));
});

app.MapPost("/decrease", (DecreaseInventoryDto request) =>
{
    var stock = stocks.FirstOrDefault(s => s.ProductId == request.ProductId);

    if (stock is null)
    {
        return Results.BadRequest(new Result<string>("Ürün için stok bulunamadý"));
    }

    if (stock.Quantity < request.Quantity)
    {
        return Results.BadRequest(new Result<string>("Yetersiz stok"));
    }

    stock.Quantity -= request.Quantity;

    return Results.Ok(new Result<string>("Stok güncellendi"));
});

app.MapDelete("/clear", () =>
{
    stocks.Clear();
    return Results.Ok("Inventory listesi temizlendi");
});

app.Run();