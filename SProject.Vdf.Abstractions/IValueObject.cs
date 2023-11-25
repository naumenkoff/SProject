namespace SProject.Vdf.Abstractions;

public interface IValueObject
{
    string? Key { get; }
    IValueObject this[string key] { get; }
}