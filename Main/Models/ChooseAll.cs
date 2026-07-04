namespace Main.Models;

class ChooseAll : Question
{
    public override void Display(int order = 0)
    {
        Console.WriteLine("Choose Many:");
        Console.WriteLine($"\n[{order}]- {this.Header}");
        Console.WriteLine($"Body: {this.Body}");
        Console.WriteLine($"Marks: {this.Marks}");

        for (int i = 0; i < this.Answers.Count; i++)
        {
            Console.WriteLine($"{i}. {this.Answers[i].Text}");
        }
    }

    public override List<string> AskAnswer()
    {
        Console.Write($"Your answers (example: 0,2 — pick from 0 to {this.Answers.Count - 1}): ");
        string input = Console.ReadLine();

        string[] parts      = input.Split(",");
        List<string> result = new List<string>();

        foreach (string part in parts)
        {
            if (!int.TryParse(part.Trim(), out int index) || index < 0 || index >= this.Answers.Count)
                throw new Exception($"Invalid. Use numbers between 0 and {this.Answers.Count - 1} separated by commas.");

            result.Add(part.Trim());
        }

        if (result.Count == 0)
            throw new Exception("You must select at least one answer.");

        return result;
    }
}