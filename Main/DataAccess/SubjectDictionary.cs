namespace Main.DataAccess;

using Main.Models;

class SubjectDictionary : BaseDictionary<Subject>
{
    public SubjectDictionary()
    {
        Path = BasePath + "subjects.txt";
        Load();
    }

    public new Subject Add(string id, Subject subject)
    {
        if (this.ContainsKey(id))
            throw new Exception($"Subject with id {id} already exists.");

        base.Add(id, subject);
        Save();

        return subject;
    }

    protected override Subject BuildObject(Dictionary<string, string> fields)
    {
        return new Subject
        {
            Id         = fields["Id"],
            Name       = fields["Name"],
            TeacherId  = fields["TeacherId"] ?? null,
            StudentIds = fields.ContainsKey("StudentIds") && fields["StudentIds"] != ""
                ? fields["StudentIds"].Split(",").ToList()
                : new List<string>()
        };
    }

    protected override void WriteObject(Subject subject, TextWriter writer)
    {
        string students = subject.StudentIds.Count > 0
            ? string.Join(",", subject.StudentIds)
            : "";

        writer.WriteLine($"Id: {subject.Id}");
        writer.WriteLine($"Name: {subject.Name}");
        writer.WriteLine($"TeacherId: {subject.TeacherId}");
        writer.WriteLine($"StudentIds: {students}");
    }
}