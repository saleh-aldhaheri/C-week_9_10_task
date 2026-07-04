namespace Main.Commands;

using Main.DataAccess;
using Main.Models;
using Main.Utilities;

class ListExamsCommand
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

        ExamDictionary exams   = new ExamDictionary(subject);
        QuestionList questions = new QuestionList(subject);

        foreach (var pair in exams)
        {
            pair.Value.LoadAnswerKey(questions);
        }

        if (exams.Count == 0)
        {
            Console.WriteLine("No exams found.");
            return;
        }

        Console.WriteLine("\n=== Exams ===");

        foreach (var pair in exams)
        {
            Exam exam = pair.Value;

            Console.WriteLine($"Id: {exam.Id}");
            Console.WriteLine($"Type: {exam.Type}");
            Console.WriteLine($"Duration: {exam.DurationMinutes} mins");
            Console.WriteLine($"Start: {exam.StartTime}");
            Console.WriteLine($"End: {exam.EndTime}");
            Console.WriteLine($"Mode: {exam.Mode}");
            Console.WriteLine($"Questions: {exam.NumberOfQuestions}");
            Console.WriteLine("---");
        }
    }
}