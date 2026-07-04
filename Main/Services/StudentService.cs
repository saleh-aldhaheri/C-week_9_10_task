namespace Main.Services;

using Main.Models;
using Main.DataAccess;
using Main.Commands;

class StudentService
{
    private Student _student;
    private SubjectDictionary _subjects;

    public StudentService(Student student, SubjectDictionary subjects)
    {
        _student  = student;
        _subjects = subjects;
    }

    public void Start()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== Student Menu ===");
            Console.WriteLine("1. View Schedule");
            Console.WriteLine("2. Take Exam");
            Console.WriteLine("3. View Results");
            Console.WriteLine("4. Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": ViewScheduleCommand.Execute(_student, _subjects);  break;
                case "2": TakeExamCommand.Execute(_student, _subjects);      break;
                case "3": ViewResultsCommand.Execute(_student, _subjects);   break;
                case "4": running = false;                                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}