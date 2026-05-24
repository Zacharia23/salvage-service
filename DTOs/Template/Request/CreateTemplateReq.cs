using SalvageCore.Enums;

namespace SalvageCore.DTOs.Template.Request;

public class CreateTemplateReq
{
    public string Name { get; set; }
    public NotificationChannels Channel { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
}