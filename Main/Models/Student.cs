namespace Main.Models;

using Main.Services; 
using Main.DataAccess;
class Student : User
{
    public override List<string> SubjectIds
    {
        get;
        set
        {
            if (value == null)
            {
                throw new Exception("SubjectIds cannot be null.");
            }

            if (value.Count > 6)
            {
                throw new Exception("Only 6 subjects allowed.");
            }

            field = value;
        }
    }    

    public void OnExamStarted(Exam exam)
    {
        Console.WriteLine($"[{Name}] Exam {exam.Type} is starting now!");
    }

    public void OnExamQueued(Exam exam)
    {
        Console.WriteLine($"[{Name}] Exam {exam.Type} starts at {exam.StartTime}");
    }

    public void OnExamFinished(Exam exam)
    {
        Console.WriteLine($"[{Name}] Exam {exam.Type} has finished.");
    }

    public override void Start(SubjectDictionary subjects)
    {
        EventMapper.RegisterToExamEvents(this, subjects);
        new StudentService(this, subjects).Start();
    }
}
