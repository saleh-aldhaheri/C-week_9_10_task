namespace Main.Services;

using Main.Models;
using Main.DataAccess;
using Main.Commands;

class AuthService
{
    private UserDictionary    _users;
    private SubjectDictionary _subjects;

    public AuthService(UserDictionary users, SubjectDictionary subjects)
    {
        _users    = users;
        _subjects = subjects;
    }

    public User Run(string role)
    {
        while (true)
        {
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.Write("Choose: ");
            string authChoice = Console.ReadLine();

            try
            {
                if (authChoice == "1")
                {
                    if (role == "teacher")
                        return RegisterCommand.Execute<Teacher>(_users, _subjects);
                    else
                        return RegisterCommand.Execute<Student>(_users, _subjects);
                }
                else if (authChoice == "2")
                {
                    if (role == "teacher")
                        return LoginCommand.Execute<Teacher>(_users);
                    else
                        return LoginCommand.Execute<Student>(_users);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} Try again.");
            }
        }
    }
}