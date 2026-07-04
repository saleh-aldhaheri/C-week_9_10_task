namespace Main.Models;

class StudentAnswer : Model
{
    public string? QuestionId { get; set; }
    public List<string> GivenAnswerIds { get; set; } = new List<string>();
}