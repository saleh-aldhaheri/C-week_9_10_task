namespace Main.DataAccess;

using Main.Models;

class ResultDictionary : BaseDictionary<ExamResult>
{
    public ResultDictionary(Subject subject, Exam exam)
    {
        Path = BasePath + $"{subject.Name}_exam_{exam.Id}_results.txt";
        Load();
    }

    public new ExamResult Add(string id, ExamResult result)
    {
        if (this.ContainsKey(id))
            throw new Exception("Result already exists.");

        base.Add(id, result);
        Save();

        return result;
    }

    protected override ExamResult BuildObject(Dictionary<string, string> fields)
    {
        ExamResult result = new ExamResult
        {
            Id         = fields["Id"],
            ExamId     = fields["ExamId"],
            StudentId  = fields["StudentId"],
            Score      = int.Parse(fields["Score"]),
            TotalMarks = int.Parse(fields["TotalMarks"]),
            TakenAt    = DateTime.Parse(fields["TakenAt"])
        };

        result.Answers = new List<StudentAnswer>();

        string answersField = fields["Answers"];

        if (!string.IsNullOrWhiteSpace(answersField))
        {
            foreach (string part in answersField.Split(" , "))
            {
                string[] data = part.Split("|");

                if (data.Length < 2) continue;

                result.Answers.Add(new StudentAnswer
                {
                    QuestionId     = data[0].Trim(),
                    GivenAnswerIds = data[1].Split(";").ToList()
                });
            }
        }

        return result;
    }

    protected override void WriteObject(ExamResult result, TextWriter writer)
    {
        writer.WriteLine($"Id: {result.Id}");
        writer.WriteLine($"ExamId: {result.ExamId}");
        writer.WriteLine($"StudentId: {result.StudentId}");
        writer.WriteLine($"Score: {result.Score}");
        writer.WriteLine($"TotalMarks: {result.TotalMarks}");
        writer.WriteLine($"TakenAt: {result.TakenAt}");

        string answers = string.Join(" , ",
            result.Answers.Select(a =>
                $"{a.QuestionId}|{string.Join(";", a.GivenAnswerIds)}"));

        writer.WriteLine($"Answers: {answers}");
    }
}