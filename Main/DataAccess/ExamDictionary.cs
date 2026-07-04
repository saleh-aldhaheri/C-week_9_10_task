namespace Main.DataAccess;

using Main.Models;
using Main.Enums;

class ExamDictionary : BaseDictionary<Exam>
{
    public ExamDictionary(Subject subject)
    {
        Path = BasePath + $"{subject.Name}_exams.txt";
        Load();
    }

    public new Exam Add(string id, Exam exam)
    {
        if (this.ContainsKey(id))
            throw new Exception($"Exam with id {id} already exists.");

        base.Add(id, exam);
        Save();

        return exam;
    }

    protected override Exam BuildObject(Dictionary<string, string> fields)
    {
        Exam exam = null;

        switch (fields["Type"].ToLower())
        {
            case "practice": exam = new Practice(); break;
            case "final":    exam = new Final();    break;
            default: return null;
        }

        exam.Id                = fields["Id"];
        exam.SubjectId         = fields["SubjectId"];
        exam.Type              = fields["Type"];
        exam.DurationMinutes   = int.Parse(fields["DurationMinutes"]);
        exam.NumberOfQuestions = int.Parse(fields["NumberOfQuestions"]);
        exam.StartTime         = DateTime.Parse(fields["StartTime"]);
        exam.EndTime           = DateTime.Parse(fields["EndTime"]);

        exam.QuestionIds = new List<string>();
        foreach (string questionId in fields["QuestionIds"].Split(","))
            exam.QuestionIds.Add(questionId.Trim());

        return exam;
    }

    protected override void WriteObject(Exam exam, TextWriter writer)
    {
        writer.WriteLine($"Id: {exam.Id}");
        writer.WriteLine($"SubjectId: {exam.SubjectId}");
        writer.WriteLine($"DurationMinutes: {exam.DurationMinutes}");
        writer.WriteLine($"NumberOfQuestions: {exam.NumberOfQuestions}");
        writer.WriteLine($"StartTime: {exam.StartTime}");
        writer.WriteLine($"EndTime: {exam.EndTime}");
        writer.WriteLine($"Type: {exam.GetType().Name}");
        writer.WriteLine($"QuestionIds: {string.Join(",", exam.QuestionIds)}");
    }
}