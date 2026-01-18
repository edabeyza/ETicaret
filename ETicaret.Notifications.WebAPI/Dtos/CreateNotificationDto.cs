
namespace ETicaret.Notifications.WebAPI.Dtos;
public sealed class CreateNotificationDto
{
    public string Message { get; set; } = default!;
    public string Type { get; set; } = "Info";
}
