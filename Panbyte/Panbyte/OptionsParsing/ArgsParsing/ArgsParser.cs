namespace Panbyte.OptionsParsing.ArgsParsing;

public class ArgsParser
{
    private string[] _argsToParse = Array.Empty<string>();
    private List<Option> _parsedOpts = new();

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

    private string GetOptArg(int currentIndex, string optionName)
    {
        if (currentIndex + 1 >= _argsToParse.Length)
        {
            throw new ArgumentException($"'{optionName}' option requires a argument");
        }

        return _argsToParse[currentIndex + 1];
    }
}