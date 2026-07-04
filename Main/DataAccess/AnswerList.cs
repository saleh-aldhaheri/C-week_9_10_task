namespace Main.DataAccess;

using Main.Models; 

class AnswerList : List<Answer>
{
    public new void Add(Answer answer)
    {
        base.Add(answer);
    }

    public void Add(string line)
    {
        Answer answer = BuildFromLine(line);
        base.Add(answer);
    }

    public Answer BuildFromLine(string line)
    {
        string[] data = line.Split(" | ");
        string id     = data[0].Split(": ")[1];
        string text   = data[1].Split(": ")[1];
        return new Answer { Id = id, Text = text };
    }

    public string GenerateId()
    {
        if (this.Count == 0)
            return "1";

        int maxId = 0;

        foreach (Answer a in this)
        {
            int currentId = int.Parse(a.Id);
            if (currentId > maxId)
                maxId = currentId;
        }

        return (maxId + 1).ToString();
    }
}