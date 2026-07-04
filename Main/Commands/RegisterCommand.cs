namespace Main.Commands;

using Main.DataAccess;
using Main.Models;
using Main.Utilities;
using BCrypt.Net;

class RegisterCommand
{
    public static T Execute<T>(UserDictionary users, SubjectDictionary subjects) where T : User, new()
    {
        string name = null;
        while (true)
        {
            Console.Write("Name: ");
            name = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name))
                break;

            Console.WriteLine("Name cannot be empty. Try again.");
        }

        string email = null;
        while (true)
        {
            Console.Write("Email: ");
            email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                Console.WriteLine("Invalid email. Try again.");
                continue;
            }

            bool emailExists = false;
            foreach (var pair in users)
            {
                if (pair.Value.Email == email)
                {
                    emailExists = true;
                    break;
                }
            }

            if (emailExists)
            {
                Console.WriteLine("Email already exists. Try again.");
                continue;
            }

            break;
        }

        string password = null;
        while (true)
        {
            Console.Write("Password: ");
            password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                Console.WriteLine("Password must be at least 6 characters. Try again.");
                continue;
            }

            break;
        }

        password = BCrypt.HashPassword(password);

        string role = typeof(T).Name.ToLower();

        List<string> subjectIds = null;
        while (true)
        {
            try
            {
                subjectIds = SelectSubject.SelectSubjects(role, subjects);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Try again.");
            }
        }

        T user          = new T();
        user.Id         = users.GenerateId();
        user.Name       = name;
        user.Email      = email;
        user.Password   = password;
        user.Role       = role;
        user.SubjectIds = subjectIds;

        users.Add(user.Id, user);

        SelectSubject.UpdateSubjects(user, subjectIds, subjects);

        Console.WriteLine("Registration successful. Welcome " + user.Name + "!");

        return user;
    }
}