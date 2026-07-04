namespace Main.Utilities;

using Main.Models;
using Main.Enums;
using Main.DataAccess;
class GenerateQuestion
{
    public static List<string> Generate(QuestionList questions, QuestionLevel level, int count)
    {
        List<Question> filtered  = new List<Question>();
        List<Question> otherType = new List<Question>();

        foreach (Question q in questions)
        {
            if (q.Level == level)
                filtered.Add(q);
            else
                otherType.Add(q);
        }

        if (filtered.Count < count)
        {
            int remaining = count - filtered.Count;

            Console.WriteLine($"Not enough {level} questions.");
            Console.WriteLine($"Found {filtered.Count} {level} questions.");
            Console.WriteLine($"Need {remaining} more questions.");

            if (filtered.Count + otherType.Count < count)
                throw new Exception($"Not enough questions in total. Only {filtered.Count + otherType.Count} available.");

            Console.Write("Do you want the remaining questions from other levels? (yes/no): ");
            string answer = Console.ReadLine();

            if (answer != "yes")
                throw new Exception("Exam creation cancelled.");

            Random random = new Random();
            for (int i = otherType.Count - 1; i > 0; i--)
            {
                int j         = random.Next(i + 1);
                Question temp = otherType[i];
                otherType[i]  = otherType[j];
                otherType[j]  = temp;
            }

            for (int i = 0; i < remaining; i++)
            {
                filtered.Add(otherType[i]);
            }
        }

        Random rand = new Random();
        for (int i = filtered.Count - 1; i > 0; i--)
        {
            int j         = rand.Next(i + 1);
            Question temp = filtered[i];
            filtered[i]   = filtered[j];
            filtered[j]   = temp;
        }

        List<string> selectedIds = new List<string>();
        for (int i = 0; i < count; i++)
        {
            selectedIds.Add(filtered[i].Id);
        }

        return selectedIds;
    }
}