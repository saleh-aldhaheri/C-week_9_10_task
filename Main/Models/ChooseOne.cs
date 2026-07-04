namespace Main.Models;

class ChooseOne : Question
{
    public override void Display(int order = 0)
    {
        Console.WriteLine("Choose One:");
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
        Console.Write($"Your answer (0 to {this.Answers.Count - 1}): ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int index) || index < 0 || index >= this.Answers.Count)
            throw new Exception($"Invalid. Please type a number between 0 and {this.Answers.Count - 1}.");

        return new List<string> { input };
    }
}