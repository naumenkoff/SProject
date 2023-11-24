namespace SProject.VDF;

public class RootObject : IValueObject
{
    public List<IValueObject> ValueObjects { get; set; } = new List<IValueObject>();
    public string? Key { get; set; }
}