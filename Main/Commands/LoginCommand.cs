namespace Main.Commands;

using Main.DataAccess;
using Main.Models;
using BCrypt.Net;

class LoginCommand
{
    public static T Execute<T>(UserDictionary users) where T : User
    {
        while (true)
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            bool emailFound = false;

            foreach (var pair in users)
            {
                if (pair.Value.Email == email)
                {
                    emailFound = true;

                    if (BCrypt.Verify(password, pair.Value.Password))
                    {
                        if (pair.Value is T typedUser)
                        {
                            Console.WriteLine("Welcome back " + typedUser.Name + "!");
                            return typedUser;
                        }

                        Console.WriteLine("Wrong role. You are not a " + typeof(T).Name + ". Try again.");
                        break;
                    }

                    Console.WriteLine("Wrong password. Try again.");
                    break;
                }
            }

            if (!emailFound)
                Console.WriteLine("Email not found. Try again.");
        }
    }
}