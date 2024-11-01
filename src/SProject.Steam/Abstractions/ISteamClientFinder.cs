namespace SProject.Steam.Abstractions;

public interface ISteamClientFinder
{
    IEnumerable<SteamClientModel> FindSteamClients();
}