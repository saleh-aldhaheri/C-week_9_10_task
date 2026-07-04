namespace Main.DataAccess;

using Main.Models;

class UserDictionary : BaseDictionary<User>
{
    public UserDictionary(string fileName)
    {
        Path = BasePath + $"{fileName}.txt";
        Load();
    }

    public new User Add(string id, User user)
    {
        if (this.ContainsKey(id))
            throw new Exception($"User with id {id} already exists.");

        base.Add(id, user);
        Save();

        return user;
    }

    protected override User BuildObject(Dictionary<string, string> fields)
    {
        User user = null;

        switch (fields["Role"])
        {
            case "student": user = new Student(); break;
            case "teacher": user = new Teacher(); break;
            default: return null;
        }

        user.Id         = fields["Id"];
        user.Name       = fields["Name"];
        user.Email      = fields["Email"];
        user.Password   = fields["Password"];
        user.Role       = fields["Role"];
        user.SubjectIds = fields.ContainsKey("SubjectIds") && fields["SubjectIds"] != ""
            ? fields["SubjectIds"].Split(",").ToList()
            : new List<string>();

        return user;
    }

    protected override void WriteObject(User user, TextWriter writer)
    {
        string subjects = user.SubjectIds.Count > 0
            ? string.Join(",", user.SubjectIds)
            : "";

        writer.WriteLine($"Id: {user.Id}");
        writer.WriteLine($"Name: {user.Name}");
        writer.WriteLine($"Email: {user.Email}");
        writer.WriteLine($"Password: {user.Password}");
        writer.WriteLine($"Role: {user.Role}");
        writer.WriteLine($"SubjectIds: {subjects}");
    }
}