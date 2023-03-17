using Panbyte.Formats;

namespace Panbyte.Converters;

public interface IConverter
{
    IFormat InputFormat { get; }
    string ConvertTo(string value, IFormat outputFormat);
}