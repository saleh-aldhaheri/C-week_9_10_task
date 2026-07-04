namespace Main;  

using Main.DataAccess;
using Main.Services;

class AppBoot
{
    public static void Start()
    {
        SubjectDictionary subjects = new SubjectDictionary();
        UserDictionary users       = new UserDictionary("users");

        Console.WriteLine("Welcome to the Exam System");
        Console.WriteLine("==========================");

        while (true)
        {
            string role;

            Console.WriteLine("1. Teacher");
            Console.WriteLine("2. Student");
            Console.WriteLine("3. Exit");
            Console.Write("Select your role: ");
            string roleChoice = Console.ReadLine();

            if (roleChoice == "1")
            {
                role = "teacher";
            }
            else if (roleChoice == "2")
            {
                role = "student";
            }
            else if (roleChoice == "3")
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Try again.");
                continue;
            }

            new AuthService(users, subjects).Run(role).Start(subjects);
        }

    }
}