namespace SProject.Steam;

/// <summary>
///     The SteamConverter class provides a set of methods for converting between various Steam ID formats.
/// </summary>
public static class SteamConverter
{
    /// <summary>
    ///     Converts a 32-bit SteamID to a 64-bit SteamID.
    /// </summary>
    /// <param name="steamID32">The 32-bit SteamID to convert.</param>
    /// <returns>The converted 64-bit SteamID.</returns>
    /// <remarks>
    ///     This method takes a 32-bit SteamID and converts it to a 64-bit SteamID.
    ///     For example, 113621430 would be converted to 76561198073887158.
    /// </remarks>
    public static long ToSteamID64(uint steamID32)
    {
        return steamID32 | WellKnownConstants.SteamID64Offset;
    }

    /// <summary>
    ///     Converts a 64-bit SteamID to a 32-bit SteamID.
    /// </summary>
    /// <param name="steamID64">The 64-bit SteamID to convert.</param>
    /// <returns>The converted 32-bit SteamID.</returns>
    /// <remarks>
    ///     This method takes a 64-bit SteamID and converts it to a 32-bit SteamID.
    ///     For example, 76561198073887158 would be converted to 113621430.
    /// </remarks>
    public static uint ToSteamID32(long steamID64)
    {
        return (uint) (steamID64 & uint.MaxValue);
    }

    /// <summary>
    ///     Returns the permanent URL for a 64-bit SteamID.
    /// </summary>
    /// <param name="steamID64">The 64-bit SteamID to get the URL for.</param>
    /// <returns>The permanent URL for the specified Steam ID.</returns>
    /// <remarks>
    ///     This method takes a 64-bit SteamID and converts it to a Permanent URL.
    ///     For example, 76561198073887158 would be converted to "https://steamcommunity.com/profiles/76561198073887158".
    /// </remarks>
    public static string ToSteamPermanentUrl(long steamID64)
    {
        return $"https://steamcommunity.com/profiles/{steamID64}";
    }

    /// <summary>
    ///     Converts a 32-bit SteamID to a Steam3ID.
    /// </summary>
    /// <param name="steamID32">The 32-bit SteamID to convert.</param>
    /// <returns>The converted Steam3ID.</returns>
    /// <remarks>
    ///     This method takes a 32-bit SteamID and converts it to a Steam3ID.
    ///     For example, 113621430 would be converted to [U:1:113621430].
    /// </remarks>
    public static string ToSteamID3(uint steamID32)
    {
        return $"[U:1:{steamID32}]";
    }

    /// <summary>
    ///     Converts a 64-bit SteamID to a SteamID.
    /// </summary>
    /// <remarks>
    ///     This method takes a 64-bit SteamID and converts it to a SteamID.
    ///     For example, 76561198073887158 would be converted to "STEAM_1:0:56810715".
    /// </remarks>
    /// <param name="steamID64">The 64-bit SteamID to convert.</param>
    /// <param name="instance">Instance of the account.</param>
    /// <param name="type">Type of account.</param>
    /// <returns>The converted SteamID.</returns>
    public static string ToSteamID(long steamID64, out int instance, out int type)
    {
        // The lowest bit represents Y.
        // Y is part of the ID number for the account. Y is either 0 or 1.
        var y = steamID64 & 0x1;

        // The next 31 bits represent the account number.
        // Z is the "account number"
        var z = (steamID64 >> 1) & 0x7FFFFFFF;

        // The next 20 bits represent the instance of the account. It is usually set to 1 for user accounts.
        instance = (int) (steamID64 >> 32) & 0xFFFFF;

        // The next 4 bits represent the type of account.
        type = (int) (steamID64 >> 52) & 0xF;

        // The next 8 bits represent the "Universe" the steam account belongs to.
        // X represents the "Universe" the steam account belongs to.
        // 0	Individual / Unspecified
        // 1	Public
        // 2	Beta
        // 3	Internal
        // 4	Dev
        // 5	RC
        var x = steamID64 >> 56;

        return $"STEAM_{x}:{y}:{z}";
    }

    /// <summary>
    ///     Converts a SteamID to a 64-bit SteamID.
    /// </summary>
    /// <remarks>
    ///     This method takes a SteamID and converts it to a 64-bit SteamID.
    ///     For example, "STEAM_1:0:56810715" would be converted to 76561198073887158.
    /// </remarks>
    /// <param name="type">The type of SteamID, either 0 or 1.</param>
    /// <param name="accountNumber">The account number.</param>
    /// <returns>The converted 64-bit SteamID.</returns>
    public static long ToSteamID64(byte type, long accountNumber)
    {
        return (accountNumber << 1) + WellKnownConstants.SteamID64Offset + type;
    }
}