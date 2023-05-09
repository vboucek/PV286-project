namespace Panbyte.OptionsParsing.ArgsParsing;

/// <summary>
/// Represents a console app switch.
/// </summary>
public class Switch
{
    /// <summary>
    /// Short switch name with prefix (e.g. '-h').
    /// </summary>
    public string? ShortSwitch { get; }
    
    /// <summary>
    /// Long switch name with prefix (e.g. '--help').
    /// </summary>
    public string LongSwitch { get; }
    
    /// <summary>
    /// Determines whether this switch has an argument.
    /// </summary>
    public bool HasArgument { get; }

    public Switch(string? shortSwitch, string longSwitch, bool hasArgument)
    {
        ShortSwitch = shortSwitch;
        LongSwitch = longSwitch;
        HasArgument = hasArgument;
    }
}