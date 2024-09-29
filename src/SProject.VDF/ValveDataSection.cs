using System.Collections;
using SProject.VDF.Collections;

namespace SProject.VDF;

public sealed class ValveDataSection(string key) : IValveDataObject, IEnumerable<ValveDataSection>
{
    private ValveDataCollection<ValveDataProperty>? _properties;
    private ValveDataCollection<ValveDataSection>? _sections;

    public ValveDataCollection<ValveDataSection> Sections =>
        _sections ?? ValveDataCollection<ValveDataSection>.Empty;

    public ValveDataCollection<ValveDataProperty> Properties =>
        _properties ?? ValveDataCollection<ValveDataProperty>.Empty;

    public ValveDataSection? this[string key] => Sections[key];

    public IEnumerator<ValveDataSection> GetEnumerator()
    {
        return Sections.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public string Key { get; init; } = key;

    public void Add(ValveDataSection valveDataSection)
    {
        _sections ??= [];
        _sections.Add(valveDataSection);
    }

    public void Add(ValveDataProperty valveDataProperty)
    {
        _properties ??= [];
        _properties.Add(valveDataProperty);
    }
}