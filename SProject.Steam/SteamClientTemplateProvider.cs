using System.Text.RegularExpressions;
using SProject.Steam.Abstractions;

namespace SProject.Steam;

public partial class SteamClientTemplateProvider : ISteamClientTemplateProvider
{
    public (Func<Match, string> Selector, Regex Template) GetTemplate(SteamClientTemplateType steamClientTemplateType)
    {
        return steamClientTemplateType switch
        {
            SteamClientTemplateType.Instance => (match => match.Groups[1].Value, SteamLibraryPattern()),
            _ => throw new ArgumentException(null, nameof(steamClientTemplateType))
        };
    }

    [GeneratedRegex("\"path\"\\s+\"([^\"]+)\"", RegexOptions.Compiled)]
    private partial Regex SteamLibraryPattern();
}