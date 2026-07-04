namespace Main.Services;

using Main.Models;
using Main.DataAccess;
using Main.Commands;

class TeacherService
{
    private Teacher _teacher;
    private SubjectDictionary _subjects;

    public TeacherService(Teacher teacher, SubjectDictionary subjects)
    {
        _teacher  = teacher;
        _subjects = subjects;
    }

    public void Start()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== Teacher Menu ===");
            Console.WriteLine("1. Create Question");
            Console.WriteLine("2. Create Exam");
            Console.WriteLine("3. List Exams");
            Console.WriteLine("4. Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": CreateQuestionsCommand.Execute(_teacher, _subjects); break;
                case "2": CreateExamCommand.Execute(_teacher, _subjects);     break;
                case "3": ListExamsCommand.Execute(_teacher, _subjects);      break;
                case "4": running = false;                                     break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}