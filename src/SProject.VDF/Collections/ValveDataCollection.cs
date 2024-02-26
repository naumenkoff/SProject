namespace SProject.VDF.Collections;

public sealed class ValveDataCollection<T> : ValveDataEnumerable<T> where T : IValveDataObject
{
    public void Add(T item)
    {
        Collection ??= new List<T>();
        Collection.Add(item);
    }
}