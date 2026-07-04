namespace Main.Models;

using BCrypt.Net;
using Main.DataAccess;
using Main.Utilities;

abstract class User : Model
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public virtual List<string> SubjectIds { get; set; } = new List<string>();

    public abstract void Start(SubjectDictionary subjects); 
}