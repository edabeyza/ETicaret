namespace ETicaret.Notifications.WebAPI.Dtos;
public sealed class NotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = default!;
    public string Type { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}