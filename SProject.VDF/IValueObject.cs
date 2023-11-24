namespace SProject.VDF;

public interface IValueObject
{
    public string? Key { get; }
    public IValueObject this[string key] { get; }
}