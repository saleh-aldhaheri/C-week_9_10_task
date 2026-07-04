namespace Main.Models;

class Subject : Model
{
    public string? Name { get; set; }
    public string? TeacherId { get; set; }
    public List<string> StudentIds { get; set; } = new List<string>();
}