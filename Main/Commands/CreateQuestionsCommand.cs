namespace Main.Commands;

using Main.DataAccess;
using Main.Models;
using Main.Enums;
using Main.Utilities;

class CreateQuestionsCommand
{
    public static void Execute(Teacher teacher, SubjectDictionary subjects)
    {
        Subject subject = null;
        try
        {
            subject = SelectSubject.PickSubject(teacher, subjects);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        QuestionList questions = new QuestionList(subject);

        int numberOfQuestions = 0;
        while (true)
        {
            Console.Write("\nHow many questions do you want to add? ");
            if (int.TryParse(Console.ReadLine(), out numberOfQuestions) && numberOfQuestions > 0)
                break;
            Console.WriteLine("Please enter a valid number.");
        }

        for (int i = 1; i <= numberOfQuestions; i++)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine($"\n--- Question {i} ---");

                    Console.Write("Header: ");
                    string header = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(header))
                        throw new Exception("Header cannot be empty.");

                    Console.Write("Body: ");
                    string body = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(body))
                        throw new Exception("Body cannot be empty.");

                    int marks = 0;
                    while (true)
                    {
                        Console.Write("Marks: ");
                        if (int.TryParse(Console.ReadLine(), out marks) && marks > 0)
                            break;
                        Console.WriteLine("Please enter a valid number.");
                    }

                    QuestionLevel level = QuestionLevel.Easy;
                    while (true)
                    {
                        Console.WriteLine("Level: 1. Easy  2. Medium  3. Hard");
                        Console.Write("Choose: ");
                        string levelChoice = Console.ReadLine();

                        if (levelChoice == "1") { level = QuestionLevel.Easy;   break; }
                        if (levelChoice == "2") { level = QuestionLevel.Medium; break; }
                        if (levelChoice == "3") { level = QuestionLevel.Hard;   break; }

                        Console.WriteLine("Invalid level. Try again.");
                    }

                    string type = "";
                    while (true)
                    {
                        Console.WriteLine("Type: 1. TrueOrFalse  2. ChooseOne  3. ChooseAll");
                        Console.Write("Choose: ");
                        string typeChoice = Console.ReadLine();

                        if (typeChoice == "1") { type = "trueorfalse"; break; }
                        if (typeChoice == "2") { type = "chooseone";   break; }
                        if (typeChoice == "3") { type = "chooseall";   break; }

                        Console.WriteLine("Invalid type. Try again.");
                    }

                    AnswerList answers = new AnswerList();

                    if (type == "trueorfalse")
                    {
                        answers.Add(new Answer { Id = "0", Text = "True"  });
                        answers.Add(new Answer { Id = "1", Text = "False" });
                    }
                    else
                    {
                        int answerCount = 0;
                        while (true)
                        {
                            Console.Write("How many answers? ");
                            if (int.TryParse(Console.ReadLine(), out answerCount) && answerCount > 1)
                                break;
                            Console.WriteLine("Please enter a valid number greater than 1.");
                        }

                        for (int j = 0; j < answerCount; j++)
                        {
                            Console.Write($"Answer {j}: ");
                            string text = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(text))
                                throw new Exception($"Answer {j} cannot be empty.");

                            answers.Add(new Answer { Id = j.ToString(), Text = text });
                        }
                    }

                    Console.WriteLine("\nAnswers:");
                    for (int j = 0; j < answers.Count; j++)
                        Console.WriteLine($"{j} - {answers[j].Text}");

                    List<int> correctIndexes = null;
                    while (true)
                    {
                        Console.Write("Correct answer index (one: 0  or  many: 0,2): ");
                        string correctInput = Console.ReadLine();

                        try
                        {
                            correctIndexes = new List<int>();

                            foreach (string part in correctInput.Split(","))
                            {
                                correctIndexes.Add(int.Parse(part.Trim()));
                            }

                            bool allValid = true;
                            foreach (int index in correctIndexes)
                            {
                                if (index < 0 || index >= answers.Count)
                                {
                                    allValid = false;
                                    break;
                                }
                            }

                            if (!allValid)
                                throw new Exception("Index out of range.");

                            break;
                        }
                        catch
                        {
                            Console.WriteLine($"Invalid. Use numbers between 0 and {answers.Count - 1}.");
                        }
                    }

                    Question question = type switch
                    {
                        "trueorfalse" => new TrueOrFalse(),
                        "chooseone"   => new ChooseOne(),
                        "chooseall"   => new ChooseAll(),
                        _             => throw new Exception("Invalid question type.")
                    };

                    question.Header               = header;
                    question.Body                 = body;
                    question.Marks                = marks;
                    question.IndexOfCorrectAnswer = correctIndexes;
                    question.Level                = level;
                    question.Answers              = answers;

                    questions.Add(question);

                    Console.WriteLine("Question saved.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.WriteLine("Let's try this question again.");
                }
            }
        }
    }
}