namespace SProject.Steam.Abstractions;

public interface ISteamInstallPathResolver<in T>
{
    string? GetInstallPath(T source);
}