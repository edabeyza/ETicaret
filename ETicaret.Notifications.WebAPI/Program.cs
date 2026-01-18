using ETicaret.Notifications.WebAPI.Dtos;
using ETicaret.Notifications.WebAPI.Models;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

List<Notification> notifications = new();

app.MapGet("/getall", () =>
{
    List<NotificationDto> response = notifications
        .Select(n => new NotificationDto
        {
            Id = n.Id,
            Message = n.Message,
            Type = n.Type,
            CreatedAt = n.CreatedAt
        }).ToList();

    return Results.Ok(new Result<List<NotificationDto>>(response));
});

app.MapPost("/create", (CreateNotificationDto request) =>
{
    Notification notification = new()
    {
        Id = Guid.NewGuid(),
        Message = request.Message,
        Type = request.Type,
        CreatedAt = DateTime.Now
    };

    notifications.Add(notification);

    return Results.Ok(new Result<string>("Bildirim baþarýyla oluþturuldu"));
});

app.MapDelete("/clear", () =>
{
    notifications.Clear();
    return Results.Ok("Notifications listesi temizlendi");
});

app.Run();
