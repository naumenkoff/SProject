namespace SProject.Steam.Abstractions;

public interface ISteamInstallPathResolver<in T>
{
    string? Resolve(T source);
}