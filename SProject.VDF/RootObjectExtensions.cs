using SProject.Vdf.Abstractions;

namespace SProject.VDF;

public static class RootObjectExtensions
{
    public static IEnumerable<IRootObject> GetSection(this IRootObject rootObject, string key)
    {
        var list = new List<IRootObject>();

        foreach (var val in rootObject.RootObjects)
        {
            if (val.Key == key) list.Add(val.Value);

            var searchNext = val.Value.GetSection(key);
            list.AddRange(searchNext);
        }

        if (rootObject.ValueObjects.Any(val => val.Key == key)) list.Add(rootObject);

        return list;
    }
}