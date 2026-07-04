namespace Main.Utilities;

using Main.DataAccess;
using Main.Models;
using Main.Enums;

class SelectSubject
{
    public static List<string> SelectSubjects(string role, SubjectDictionary subjects)
    {
        int maxSubjects = role.ToLower() == "teacher" ? 4 : 6;

        if (role.ToLower() == "teacher")
            Console.WriteLine("Select subjects you will teach (max 4):");
        else
            Console.WriteLine("Select your study subjects (max 6):");

        Dictionary<string, string> display = new Dictionary<string, string>();
        int counter = 1;

        foreach (var pair in subjects)
        {
            Subject subject = pair.Value;

            if (role.ToLower() == "teacher" && !string.IsNullOrEmpty(subject.TeacherId))
                continue;

            display.Add(counter.ToString(), subject.Id);
            Console.WriteLine($"{counter} - {subject.Name}");
            counter++;
        }

        if (display.Count == 0)
            throw new Exception("No subjects available.");

        List<string> selectedSubjectIds = new List<string>();

        while (selectedSubjectIds.Count < maxSubjects)
        {
            Console.WriteLine("Pick a number (or press Enter to finish):");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
                break;

            if (!display.ContainsKey(input))
                throw new Exception("Invalid selection.");

            string subjectId = display[input];

            if (selectedSubjectIds.Contains(subjectId))
                throw new Exception("You already selected this subject.");

            selectedSubjectIds.Add(subjectId);
            Console.WriteLine($"Subject added. ({selectedSubjectIds.Count}/{maxSubjects})");
        }

        if (selectedSubjectIds.Count == 0)
            throw new Exception("You must select at least one subject.");

        return selectedSubjectIds;
    }

    public static void UpdateSubjects(User user, List<string> subjectIds, SubjectDictionary subjects)
    {
        foreach (string subjectId in subjectIds)
        {
            if (user is Teacher)
                subjects[subjectId].TeacherId = user.Id;
            else if (user is Student)
                subjects[subjectId].StudentIds.Add(user.Id);
        }

        subjects.Save();
    }

    public static Subject PickSubject(User user, SubjectDictionary subjects)
    {
        Console.WriteLine("\nSelect Subject:");

        Dictionary<string, string> display = new Dictionary<string, string>();
        int counter = 1;

        foreach (var pair in subjects)
        {
            Subject subject = pair.Value;

            if (user is Teacher && subject.TeacherId != user.Id)
                continue;

            if (user is Student && !subject.StudentIds.Contains(user.Id))
                continue;

            display.Add(counter.ToString(), subject.Id);
            Console.WriteLine($"{counter} - {subject.Name}");
            counter++;
        }

        if (display.Count == 0)
            throw new Exception("You have no subjects.");

        while (true)
        {
            Console.Write("Choose: ");
            string input = Console.ReadLine();

            if (display.ContainsKey(input))
                return subjects[display[input]];

            Console.WriteLine("Invalid choice. Try again.");
        }
    }

    public static (Exam exam, Subject subject) SelectExam(Student student, SubjectDictionary subjects)
    {
        Dictionary<string, Exam> available       = new Dictionary<string, Exam>();
        Dictionary<string, Subject> examSubjects = new Dictionary<string, Subject>();
        int counter = 1;

        foreach (string subjectId in student.SubjectIds)
        {
            if (!subjects.ContainsKey(subjectId)) continue;

            Subject subject       = subjects[subjectId];
            ExamDictionary exams  = new ExamDictionary(subject);
            QuestionList questions = new QuestionList(subject);

            foreach (var examPair in exams)
            {
                Exam exam = examPair.Value;

                if (exam.Mode != ExamMode.Starting) continue;

                ResultDictionary results = new ResultDictionary(subject, exam);
                bool alreadyTook        = false;

                foreach (var resultPair in results)
                {
                    if (resultPair.Value.StudentId == student.Id)
                    {
                        alreadyTook = true;
                        break;
                    }
                }

                if (alreadyTook) continue;

                exam.LoadAnswerKey(questions);

                available[counter.ToString()]    = exam;
                examSubjects[counter.ToString()] = subject;
                Console.WriteLine($"{counter}. {subject.Name} - {exam.Type}");
                counter++;
            }
        }

        if (available.Count == 0)
            throw new Exception("No exams available right now.");

        while (true)
        {
            Console.Write("Choose exam: ");
            string input = Console.ReadLine();

            if (available.ContainsKey(input))
                return (available[input], examSubjects[input]);

            Console.WriteLine("Invalid choice. Try again.");
        }
    }
}