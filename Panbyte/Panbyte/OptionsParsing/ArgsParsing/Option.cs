namespace Panbyte.OptionsParsing.ArgsParsing;

/// <summary>
/// Represents a parsed option from the CLI.
/// </summary>
public class Option
{
    /// <summary>
    /// Respective switch.
    /// </summary>
    public Switch Switch { get; }
    
    /// <summary>
    /// Argument of the option.
    /// </summary>
    public string? Argument { get; }

    public Option(Switch sw, string? argument)
    {
        Switch = sw;
        Argument = argument;
    }
}