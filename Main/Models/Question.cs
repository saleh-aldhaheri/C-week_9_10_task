namespace Main.Models;

using Main.DataAccess;
using Main.Enums;

abstract class Question : Model
{
    public string? Header { get; set; }
    public string? Body { get; set; }
    public int Marks { get; set; }
    public List<int> IndexOfCorrectAnswer { get; set; } = new List<int>();
    public QuestionLevel Level { get; set; }
    public AnswerList Answers { get; set; } = new AnswerList();

    public abstract void Display(int order = 0);
    public abstract  List<string> AskAnswer(); 
}