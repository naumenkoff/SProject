namespace SProject.VDF;

public static class ValueObjectExtensions
{
    public static RootObject? AsRootObject(this IValueObject valueObject)
    {
        return valueObject as RootObject;
    }

    public static ValueObject? AsValueObject(this IValueObject valueObject)
    {
        return valueObject as ValueObject;
    }

    public static long AsLong(this IValueObject valueObject)
    {
        return valueObject is not ValueObject value ? -1 : long.Parse(value.Value);
    }
    
    public static long AsInt(this IValueObject valueObject)
    {
        return valueObject is not ValueObject value ? -1 : int.Parse(value.Value);
    }

    public static bool AsBool(this IValueObject valueObject)
    {
        return valueObject is ValueObject { Value: "1" };
    }
}