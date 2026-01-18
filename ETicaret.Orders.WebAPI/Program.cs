using ETicaret.Orders.WebAPI.Context;
using ETicaret.Orders.WebAPI.Dtos;
using ETicaret.Orders.WebAPI.Models;
using ETicaret.Orders.WebAPI.Options;
using System.Net.Http.Json;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/getall", async (MongoDbContext context, IConfiguration configuration) =>
{
    var items = context.GetCollection<Order>("Orders");

    var orders = await items.Find(item => true).ToListAsync();

    List<OrderDto> orderDtos = new();

    Result<List<ProductDto>>? products = new();

    HttpClient httpClient = new();
    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var message = await httpClient.GetAsync(productsEndpoint);

    if (message.IsSuccessStatusCode)
    {
        products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>();
    }

    foreach (var order in orders)
    {
        var product = products?.Data?.FirstOrDefault(p => p.Id == order.ProductId);
        OrderDto orderDto = new()
        {
            Id = order.Id,
            CreatAt = order.CreatAt,
            ProductId = order.ProductId,
            Quantity = order.Quantity,
            Price = order.Price,
            ProductName = product?.Name ?? "Ürün bilgisi bulunamadý"
        };

        orderDtos.Add(orderDto);
    }

    return Results.Ok(new Result<List<OrderDto>>(orderDtos));

});

app.MapPost("/create", async (MongoDbContext context, List<CreateOrderDto> request) =>
{
    var items = context.GetCollection<Order>("Orders");
    List<Order> orders = new();
    foreach (var item in request)
    {
        Order order = new()
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            Price = item.Price,
            CreatAt = DateTime.Now,
        };

        orders.Add(order);
    }

    await items.InsertManyAsync(orders);

    using var httpClient = new HttpClient();

    foreach (var order in orders)
    {
        var decreaseBody = new
        {
            productId = order.ProductId,
            quantity = order.Quantity
        };

        await httpClient.PostAsJsonAsync("http://inventory:8080/decrease", decreaseBody);
    }

    var notificationBody = new
    {
        message = "Yeni sipariþ oluþturuldu.",
        type = "Order"
    };

    await httpClient.PostAsJsonAsync("http://notifications:8080/create", notificationBody);


    return Results.Ok(new Result<string>("Sipariþ baþarýyla oluþturuldu"));
});

app.MapDelete("/clear", async (MongoDbContext context) =>
{
    var items = context.GetCollection<Order>("Orders");
    await items.DeleteManyAsync(_ => true);
    return Results.Ok("Orders koleksiyonu temizlendi");
});


app.Run();
