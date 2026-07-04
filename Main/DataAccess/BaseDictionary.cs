namespace Main.DataAccess;

using Main.Models;

abstract class BaseDictionary<T> : Dictionary<string, T>
    where T : Model
{
    protected static readonly string BasePath = "/home/saleh/Desktop/Learning/C#/week10/week_9_10_task/Main/data/";

    public string? Path { get; set; }

    protected void Load()
    {
        if (!File.Exists(Path)) return;

        using (TextReader reader = new StreamReader(Path))
        {
            string line;
            var fields = new Dictionary<string, string>();

            while ((line = reader.ReadLine()) != null)
            {
                if (line == "===")
                {
                    T obj = BuildObject(fields);
                    if (obj != null)
                        base.Add(obj.Id, obj);
                    fields.Clear();
                }
                else
                {
                  string[] parts = line.Split(": ", 2);

                   if (parts.Length == 2)
                   {
                        fields[parts[0]] = parts[1].Trim();
                   }
                   else if (parts.Length == 1 && parts[0].Contains(":"))
                   {
                        string key = parts[0].Replace(":", "").Trim();
                        fields[key] = "";
                    }
                }
            }
        }
    }

    public void Save()
    {
        using (TextWriter writer = new StreamWriter(Path, append: false))
        {
            foreach (var pair in this)
            {
                WriteObject(pair.Value, writer);
                writer.WriteLine("===");
            }
        }
    }

    public string GenerateId()
    {
        if (this.Count == 0)
            return "1";

        int maxId = 0;

        foreach (var pair in this)
        {
            int currentId = int.Parse(pair.Key);

            if (currentId > maxId)
                maxId = currentId;
        }

        return (maxId + 1).ToString();
    }

    protected abstract T BuildObject(Dictionary<string, string> fields);
    protected abstract void WriteObject(T obj, TextWriter writer);
}