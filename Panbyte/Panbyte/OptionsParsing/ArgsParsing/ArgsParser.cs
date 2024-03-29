namespace Panbyte.OptionsParsing.ArgsParsing;

/// <summary>
/// Parser for obtaining options from console arguments.
/// </summary>
public class ArgsParser
{
    private string[] _argsToParse = Array.Empty<string>();
    private List<Option> _parsedOpts = new();

    /// <summary>
    /// Parses CLI arguments given to the program. Supported switches can be supplied as a parameter.
    /// </summary>
    /// <param name="args">Program arguments.</param>
    /// <param name="switches">Supported switches of the program.</param>
    /// <returns>List of program options, each option contains switch with its respective argument, if it has one.</returns>
    /// <exception cref="ArgumentException">when parsed switch is not found in supported switches or switch with argument doesn't have one.</exception>
    public List<Option> ParseArgs(string[] args, ICollection<Switch> switches)
    {
        _argsToParse = args;
        _parsedOpts = new List<Option>();
        var i = 0;

        while (i < _argsToParse.Length)
        {
            var sw = switches.FirstOrDefault(x => x.ShortSwitch != null && x.ShortSwitch.Equals(_argsToParse[i]));
            string? argument = null;
            string currentToken = _argsToParse[i];

            if (sw is not null)
            {
                if (sw.HasArgument)
                {
                    argument = GetOptArg(i, sw.ShortSwitch!);
                    i++;
                }

                _parsedOpts.Add(new Option(sw, argument));
                i++;
                continue;
            }

            sw = switches.FirstOrDefault(x => x != null && currentToken.StartsWith(x.LongSwitch), null);

            if (sw is not null)
            {
                if (sw.HasArgument)
                {
                    argument = currentToken[(currentToken.IndexOf('=') + 1)..];
                }

                _parsedOpts.Add(new Option(sw, argument));
                i++;
                continue;
            }

            // Invalid opt
            throw new ArgumentException($"'{currentToken}' is an invalid argument.");
        }

        return _parsedOpts;
    }

    /// <summary>
    /// Gets argument after given index from args array.
    /// </summary>
    /// <param name="currentIndex">Current index in the args array.</param>
    /// <param name="optionName">Name of the current switch.</param>
    /// <returns>Argument of the current switch.</returns>
    /// <exception cref="ArgumentException">when argument is out of array boundaries.</exception>
    private string GetOptArg(int currentIndex, string optionName)
    {
        if (currentIndex + 1 >= _argsToParse.Length)
        {
            throw new ArgumentException($"'{optionName}' option requires a argument");
        }

        return _argsToParse[currentIndex + 1];
    }
}