namespace Main.Commands;

using Main.DataAccess;
using Main.Models;
using Main.Enums;
using Main.Utilities;

class CreateExamCommand
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

        if (questions.Count == 0)
        {
            Console.WriteLine("No questions found. Please create questions first.");
            return;
        }

        string type = "";
        while (true)
        {
            Console.WriteLine("\nExam Type: 1. Practice  2. Final");
            Console.Write("Choose: ");
            string typeChoice = Console.ReadLine();

            if (typeChoice == "1") { type = "practice"; break; }
            if (typeChoice == "2") { type = "final";    break; }

            Console.WriteLine("Invalid choice. Try again.");
        }

        ExamDictionary exams = new ExamDictionary(subject);

        foreach (var pair in exams)
        {
            Exam exam = pair.Value;

            if (exam.Type == type && exam.Mode != ExamMode.Finished)
            {
                Console.WriteLine("There is already an active exam of this type.");
                return;
            }
        }

        int duration = 0;
        while (true)
        {
            Console.Write("Duration in minutes: ");
            if (int.TryParse(Console.ReadLine(), out duration) && duration > 0)
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

            Console.WriteLine("Invalid choice. Try again.");
        }

        int count = 0;
        while (true)
        {
            Console.Write("How many questions: ");
            if (int.TryParse(Console.ReadLine(), out count) && count > 0)
                break;
            Console.WriteLine("Please enter a valid number.");
        }

        List<string> questionIds = null;
        try
        {
            questionIds = GenerateQuestion.Generate(questions, level, count);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        DateTime startTime;
        while (true)
        {
            Console.Write("Start time (format: yyyy-MM-dd HH:mm): ");
            string timeInput = Console.ReadLine();

            if (DateTime.TryParse(timeInput, out startTime))
                break;

            Console.WriteLine("Invalid date format. Try again.");
        }

        Exam newExam = type == "practice" ? new Practice() : new Final();

        newExam.Id                = exams.GenerateId();
        newExam.SubjectId         = subject.Id;
        newExam.Type              = type;
        newExam.DurationMinutes   = duration;
        newExam.QuestionIds       = questionIds;
        newExam.NumberOfQuestions = questionIds.Count;
        newExam.StartTime         = startTime;
        newExam.EndTime           = startTime.AddMinutes(duration);

        exams.Add(newExam.Id, newExam);

        Console.WriteLine("Exam created successfully.");
    }
}