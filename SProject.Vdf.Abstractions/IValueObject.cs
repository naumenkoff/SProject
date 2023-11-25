namespace SProject.Vdf.Abstractions;

public interface IValueObject
{
    string? Key { get; }
    IValueObject this[string key] { get; }
    T GetValueObject<T>(string key) where T : IValueObject;
}