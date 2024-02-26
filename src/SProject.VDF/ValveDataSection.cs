using System.Collections;
using SProject.VDF.Collections;

namespace SProject.VDF;

public sealed class ValveDataSection : IValveDataObject, IEnumerable<ValveDataSection>
{
    private ValveDataCollection<ValveDataProperty>? _properties;
    private ValveDataCollection<ValveDataSection>? _sections;

    public ValveDataSection(string key)
    {
        Key = key;
    }

    public ValveDataEnumerable<ValveDataProperty> Properties =>
        _properties ?? ValveDataEnumerable<ValveDataProperty>.Empty;

    public ValveDataEnumerable<ValveDataSection> Sections =>
        _sections ?? ValveDataEnumerable<ValveDataSection>.Empty;

    public ValveDataSection? this[string key] => Sections[key];

    public IEnumerator<ValveDataSection> GetEnumerator()
    {
        return Sections.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string Key { get; init; }

    public void Add<T>(T item) where T : IValveDataObject
    {
        switch (item)
        {
            case ValveDataSection section:
                _sections ??= new ValveDataCollection<ValveDataSection>();
                _sections.Add(section);
                break;
            case ValveDataProperty property:
                _properties ??= new ValveDataCollection<ValveDataProperty>();
                _properties.Add(property);
                break;
            default:
                throw new InvalidOperationException(
                    $"'{nameof(Add)}<{typeof(T).Name}>' doesn't support adding objects of type '{typeof(T)}'.");
        }
    }
}