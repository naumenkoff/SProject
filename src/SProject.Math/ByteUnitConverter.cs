namespace SProject.Math;

public static class ByteUnitConverter
{
    public static long MegabytesToBytes(double megabytes)
    {
        return (long)(megabytes * 1024 * 1024);
    }
}