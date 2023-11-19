namespace SProject.Steam;

public class SteamClientNotFoundException : Exception
{
    public SteamClientNotFoundException(string message) : base(message) { }
}