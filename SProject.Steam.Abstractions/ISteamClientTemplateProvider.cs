using System.Text.RegularExpressions;

namespace SProject.Steam.Abstractions;

public interface ISteamClientTemplateProvider
{
    (Func<Match, string> Selector, Regex Template) GetTemplate(SteamClientTemplateType steamClientTemplateType);
}