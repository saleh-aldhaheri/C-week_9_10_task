namespace Main.Models;

class TrueOrFalse : Question
{
    public override void Display(int order = 0)
    {
        Console.WriteLine("True Or False:");
        Console.WriteLine($"\n[{order}]- {this.Header}");
        Console.WriteLine($"Body: {this.Body}");
        Console.WriteLine($"Marks: {this.Marks}");
        Console.WriteLine("0. True");
        Console.WriteLine("1. False");
    }

    public override List<string> AskAnswer()
    {
        Console.Write("Your answer (0 for True, 1 for False): ");
        string input = Console.ReadLine();

        if (input != "0" && input != "1")
            throw new Exception("Invalid. Please type 0 or 1 only.");

        return new List<string> { input };
    }
}