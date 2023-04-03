namespace Panbyte.ArgParsing;

/// <summary>
/// Options for printing Panbyte help.
/// </summary>
public class HelpOnlyOptions : IOptions
{
    public bool Help => true;
}