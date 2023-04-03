namespace Panbyte.OptionsParsing.ArgsParsing;

public class Switch
{
    public string? ShortSwitch { get; }
    public string LongSwitch { get; }
    public bool HasArgument { get; }

    public Switch(string? shortSwitch, string longSwitch, bool hasArgument)
    {
        ShortSwitch = shortSwitch;
        LongSwitch = longSwitch;
        HasArgument = hasArgument;
    }
}