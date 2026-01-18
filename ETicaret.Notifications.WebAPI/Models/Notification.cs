namespace ETicaret.Notifications.WebAPI.Models;
public sealed class Notification
{
    public Guid Id { get; set; }
    public string Message { get; set; } = default!;
    public string Type { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
