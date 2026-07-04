namespace Main.Commands;

using Main.DataAccess;
using Main.Models;
using Main.Enums;

class ViewScheduleCommand
{
    public static void Execute(Student student, SubjectDictionary subjects)
    {
        Console.WriteLine("\n=== Upcoming Exams ===");

        bool found = false;

        foreach (string subjectId in student.SubjectIds)
        {
            if (!subjects.ContainsKey(subjectId)) continue;

            Subject subject      = subjects[subjectId];
            ExamDictionary exams = new ExamDictionary(subject);

            foreach (var pair in exams)
            {
                Exam exam = pair.Value;

                if (exam.Mode == ExamMode.Finished) continue;

                Console.WriteLine($"Subject: {subject.Name}");
                Console.WriteLine($"Type: {exam.Type}");
                Console.WriteLine($"Start: {exam.StartTime}");
                Console.WriteLine($"End: {exam.EndTime}");
                Console.WriteLine($"Duration: {exam.DurationMinutes} mins");
                Console.WriteLine($"Mode: {exam.Mode}");
                Console.WriteLine("---");

                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No upcoming exams.");
    }
}