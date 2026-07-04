namespace Main.Commands;

using Main.DataAccess;
using Main.Models;

class ViewResultsCommand
{
    public static void Execute(Student student, SubjectDictionary subjects)
    {
        Console.WriteLine("\n=== Your Results ===");

        bool found = false;

        foreach (string subjectId in student.SubjectIds)
        {
            if (!subjects.ContainsKey(subjectId)) continue;

            Subject subject      = subjects[subjectId];
            ExamDictionary exams = new ExamDictionary(subject);

            foreach (var examPair in exams)
            {
                Exam exam                = examPair.Value;
                ResultDictionary results = new ResultDictionary(subject, exam);

                foreach (var resultPair in results)
                {
                    ExamResult result = resultPair.Value;

                    if (result.StudentId != student.Id) continue;

                    Console.WriteLine($"Subject: {subject.Name}");
                    Console.WriteLine($"Exam Type: {exam.Type}");
                    Console.WriteLine($"Score: {result.Score} / {result.TotalMarks}");
                    Console.WriteLine($"Taken At: {result.TakenAt}");
                    Console.WriteLine("---");

                    found = true;
                }
            }
        }

        if (!found)
            Console.WriteLine("No results found.");
    }
}