namespace SalvageCore.DTOs.Template.Response;

public class TemplateListResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Channel { get; set; }
    public string Content { get; set; }
    public string Subject { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}