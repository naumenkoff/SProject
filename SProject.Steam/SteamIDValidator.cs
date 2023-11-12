namespace SProject.Steam;

public class SteamIDValidator
{
    public bool IsSteamID64(string id)
    {
        return long.TryParse(id, out var steamID64) && IsSteamID64(steamID64);
    }

    public bool IsSteamID64(long id)
    {
        return id is >= WellKnownConstants.SteamID64Offset and <= WellKnownConstants.SteamID64MaximumValue;
    }

    public bool IsSteamID32(string id)
    {
        return uint.TryParse(id, out var steamID32) && IsSteamID32(steamID32);
    }

    public bool IsSteamID32(uint steamID32)
    {
        return steamID32 != 0;
    }
}