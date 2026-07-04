namespace Main.DataAccess;

using Main.Models; 
 
abstract class BaseList<T> : List<T>
{
    protected static readonly string BasePath = "/home/saleh/Desktop/Learning/C#/week10/week_9_10_task/Main/data/";
    public string Path { get; set; }

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
                    base.Add(obj);
                    fields.Clear();
                }
                else
                {
                    string[] parts = line.Split(": ", 2); 
                    if (parts.Length == 2)
                        fields[parts[0]] = parts[1];
                }
            }
        }
    }

    protected void Save()
    {
        using (TextWriter writer = new StreamWriter(Path, append: false))
        {
            foreach (T obj in this)
            {
                WriteObject(obj, writer);
                writer.WriteLine("===");
            }
        }
    }
    public string GenerateId()
    {
        if (this.Count == 0)
            return "1";

        int maxId = 0;

        foreach (T obj in this)
        {
            Model model = obj as Model;
            int currentId = int.Parse(model.Id);

            if (currentId > maxId)
                maxId = currentId;
        }

        return (maxId + 1).ToString();
    }
    protected abstract T BuildObject(Dictionary<string, string> fields);
    protected abstract void WriteObject(T obj, TextWriter writer);
}