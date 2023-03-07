using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.ArgParsing;

public class ArgParser
{
    private List<string> _argsToParse = new();

    public Options ParseArguments(string[] args)
    {
        _argsToParse = new List<string>(args);
        var newOptions = ParseMandatoryOptions();
        ParseOptionalOptions(newOptions);
        return newOptions;
    }

    private Options ParseMandatoryOptions()
    {
        IFormat? inputFormat = null;
        IFormat? outputFormat = null;

        int i = 0;

        List<string> remainingArgs = new(); 

        while(i < _argsToParse.Count)
        {
            switch (_argsToParse[i])
            {
                case "-f":
                    inputFormat = ParseFormat(GetOptionArg(i, "-f"));
                    i++;
                    break;
                
                case var s when s.StartsWith("--from="):
                    inputFormat = ParseFormat(s[(s.IndexOf('=') + 1)..]);
                    break;
                
                case "-t":
                    outputFormat = ParseFormat(GetOptionArg(i, "-t"));
                    i++;
                    break;
                
                case var s when s.StartsWith("--to="):
                    outputFormat = ParseFormat(s[(s.IndexOf('=') + 1)..]);
                    break;
                
                default:
                    remainingArgs.Add(_argsToParse[i]);
                    break;
            }
            i++;
        }

        if (inputFormat is null || outputFormat is null)
        {
            throw new ArgumentException("Missing format options");
        }

        _argsToParse = remainingArgs;
        return new Options(inputFormat, outputFormat);
    }
    
    private void ParseOptionalOptions(Options options)
    {
        int i = 0;
        
        while(i < _argsToParse.Count)
        {
            switch (_argsToParse[i])
            {
                case "-h" or "--help":
                    options.Help = true;
                    break;
                
                case "-i":
                    options.InputFilePath = GetOptionArg(i, "-i");
                    i++;
                    break;
                
                case "-o":
                    options.OutputFilePath = GetOptionArg(i, "-o");
                    i++;
                    break;
                
                case "-d":
                    options.Delimiter = GetOptionArg(i, "-d");
                    i++;
                    break;
                
                case var s when s.StartsWith("--from-options="):
                    ParseInputFormatOption(options.InputFormat, s[(s.IndexOf('=') + 1)..]);
                    break;
                
                case var s when s.StartsWith("--to-options="):
                    ParseOutputFormatOption(options.OutputFormat, s[(s.IndexOf('=') + 1)..]);
                    break;
                
                case var s when s.StartsWith("--input="):
                    options.InputFilePath = s[(s.IndexOf('=') + 1)..];
                    break;
                
                case var s when s.StartsWith("--output="):
                    options.OutputFilePath = s[(s.IndexOf('=') + 1)..];
                    break;
                
                case var s when s.StartsWith("--delimiter="):
                    options.Delimiter = s[(s.IndexOf('=') + 1)..];
                    break;

                default:
                    throw new ArgumentException($"Invalid argument '{_argsToParse[i]}'");
            }
            i++;
        }
    }
    
    private IFormat ParseFormat(string format) =>
        format switch
        {
            "bytes" => new Bytes(),
            "hex" => new Hex(),
            "int" => new Int(),
            "bits" => new Bits(),
            "array" => new ByteArray(),
            _ => throw new ArgumentException($"Invalid format type '{format}'"),
        };

    private string GetOptionArg(int currentIndex, string optionName)
    {
        if (currentIndex + 1 >= _argsToParse.Count)
        {
            throw new ArgumentException($"'{optionName}' option requires a argument");
        }

        return _argsToParse[currentIndex + 1];
    }

    private void ParseInputFormatOption(IFormat inputFormat, string option)
    {
        switch (inputFormat)
        {
            case Int integer:
                integer.Endianness = option switch
                {
                    "big" => Endianness.BigEndian,
                    "little" => Endianness.LittleEndian,
                    _ => throw new ArgumentException(
                        $"Invalid input format option '{option}' for format '{integer}'"),
                };
                break;
            
            case Bits bits:
                bits.BitPadding = option switch
                {
                    "left" => BitPadding.Left,
                    "right" => BitPadding.Right,
                    _ => throw new ArgumentException(
                        $"Invalid input format option '{option}' for format '{bits}'"),
                };
                break;
            
            default:
                throw new ArgumentException(
                    $"Invalid input format option '{option}' for format '{inputFormat}'");
        }
    }

    private void ParseOutputFormatOption(IFormat outputFormat, string option)
    {
        switch (outputFormat)
        {
            case Int integer:
                integer.Endianness = option switch
                {
                    "big" => Endianness.BigEndian,
                    "little" => Endianness.LittleEndian,
                    _ => throw new ArgumentException(
                        $"Invalid output format option '{option}' for format '{integer}'"),
                };
                break;
            
            case ByteArray array:
                switch (option)
                {
                    case "0x":
                        array.ArrayFormat = ArrayFormat.Hex;
                        break;
                    
                    case "0":
                        array.ArrayFormat = ArrayFormat.Decimal;
                        break;
                    
                    case "a":
                        array.ArrayFormat = ArrayFormat.Char;
                        break;
                    
                    case "0b":
                        array.ArrayFormat = ArrayFormat.Binary;
                        break;
                    
                    case "{" or "}" or "{}":
                        array.Brackets = Brackets.Curly;
                        break;
                    
                    case "(" or ")" or "()":
                        array.Brackets = Brackets.Regular;
                        break;
                    
                    case "[" or "]" or "[]":
                        array.Brackets = Brackets.Square;
                        break;
                    
                    default:
                        throw new ArgumentException(
                            $"Invalid output format option '{option}' for format '{outputFormat}'");
                }
                break;
            
            default:
                throw new ArgumentException(
                    $"Invalid output format option '{option}' for format '{outputFormat}");
        }
    }
}