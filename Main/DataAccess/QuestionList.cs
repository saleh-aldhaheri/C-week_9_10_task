namespace Main.DataAccess;

using Main.Models;
using Main.Enums;

class QuestionList : BaseList<Question>
{
    public QuestionList(Subject subject)
    {
        Path = BasePath + $"{subject.Name}_questions.txt";
        Load();
    }

    public new Question Add(Question question)
    {
        if (question.Id == null)
            question.Id = GenerateId();

        base.Add(question);
        Save();

        return question;
    }

    protected override Question BuildObject(Dictionary<string, string> fields)
    {
        string type = fields["Type"];
        Question q  = null;

        switch (type)
        {
            case "TrueOrFalse": q = new TrueOrFalse(); break;
            case "ChooseOne":   q = new ChooseOne();   break;
            case "ChooseAll":   q = new ChooseAll();   break;
            default: return null;
        }

        q.Id     = fields["Id"];
        q.Header = fields["Header"];
        q.Body   = fields["Body"];
        q.Marks  = int.Parse(fields["Marks"]);
        q.Level  = Enum.Parse<QuestionLevel>(fields["Level"]);

        q.IndexOfCorrectAnswer = fields["IndexOfCorrectAnswer"]
            .Split(",")
            .Select(x => int.Parse(x.Trim()))
            .ToList();

        q.Answers = new AnswerList();

        string[] stringAnswerList = fields["Answers"].Split(" , ");
        foreach (string answer in stringAnswerList)
        {
            q.Answers.Add(answer);
        }

        return q;
    }

    protected override void WriteObject(Question question, TextWriter writer)
    {
        writer.WriteLine($"Id: {question.Id}");
        writer.WriteLine($"Header: {question.Header}");
        writer.WriteLine($"Body: {question.Body}");
        writer.WriteLine($"Marks: {question.Marks}");
        writer.WriteLine($"Level: {question.Level}");
        writer.WriteLine($"Type: {question.GetType().Name}");
        writer.WriteLine($"IndexOfCorrectAnswer: {string.Join(",", question.IndexOfCorrectAnswer)}");

        string stringAnswerList = string.Join(" , ",
            question.Answers.Select(a => $"Id: {a.Id} | Text: {a.Text}"));

        writer.WriteLine($"Answers: {stringAnswerList}");
    }
}