using ETicaret.ShoppingCarts.WebAPI.Context;
using ETicaret.ShoppingCarts.WebAPI.Dtos;
using ETicaret.ShoppingCarts.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/getall", async (ApplicationDbContext context, IConfiguration configuration, CancellationToken cancellationToken) =>
{
    List<ShoppingCart> shoppingCarts = await context.ShoppingCarts.ToListAsync(cancellationToken);
    HttpClient client = new HttpClient();

    string productsEndpoint = $"http://{configuration.GetSection("HttpRequest:Products").Value}/getall";
    var message = await client.GetAsync(productsEndpoint);

    Result<List<ProductDto>>? products = new();

    if (message.IsSuccessStatusCode)
    {
        products = await message.Content.ReadFromJsonAsync<Result<List<ProductDto>>>();
    }

    List<ShoppingCartDto> response = shoppingCarts.Select(s => new ShoppingCartDto()
    {
        Id = s.Id,
        ProductId = s.ProductId,
        Quantity = s.Quantity,
        ProductName = products!.Data!.First(p => p.Id == s.ProductId).Name,
        ProductPrice = products.Data!.First(p => p.Id == s.ProductId).Price
    }).ToList();


    return new Result<List<ShoppingCartDto>>(response);

});

app.MapPost("/create", async (CreateShoppingCartDto request, ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    ShoppingCart shoppingCart = new()
    {
        ProductId = request.ProductId,
        Quantity = request.Quantity
    };

    await context.AddAsync(shoppingCart, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(new Result<string>("Ürün sepete baþarýyla eklendi"));
});

using (var scoped = app.Services.CreateScope())
{
    var srv = scoped.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
