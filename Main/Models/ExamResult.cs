namespace Main.Models;

class ExamResult : Model
{
    public string? ExamId { get; set; }
    public string? StudentId { get; set; }
    public int Score { get; set; }
    public int TotalMarks { get; set; }
    public DateTime TakenAt { get; set; }
    public List<StudentAnswer> Answers { get; set; } = new List<StudentAnswer>();
}