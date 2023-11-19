namespace SProject.Steam.Abstractions;

public interface ISteamClientFinder
{
    SteamClientModel? FindSteamClient();
}