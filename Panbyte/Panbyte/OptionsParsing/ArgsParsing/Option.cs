namespace Panbyte.OptionsParsing.ArgsParsing;

public class Option
{
    public Switch Switch { get; }
    public string? Argument { get; }

    public Option(Switch sw, string? argument)
    {
        Switch = sw;
        Argument = argument;
    }
}