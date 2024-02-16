namespace SProject.Steam;

public interface ISteamInstallPathResolver<in T>
{
    string? GetInstallPath(T source);
}