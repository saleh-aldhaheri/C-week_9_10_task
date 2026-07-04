namespace Main.Models;

using Main.DataAccess; 
using Main.Services;
class Teacher : User
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

            if (value.Count > 4)
            {
                throw new Exception("Only 4 subjects allowed.");
            }

            field = value;
        }
    }

    public override void Start(SubjectDictionary subjects)
    {
        new TeacherService(this, subjects).Start(); 
    }
}