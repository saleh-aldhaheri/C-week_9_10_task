namespace Main.Models;
abstract class Model
{
  public string? Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Model other)
            return Id == other.Id;
        return false;
    }

    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

    public override string ToString()
    {
        return $"{this.GetType().Name} | Id: {Id}";
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}