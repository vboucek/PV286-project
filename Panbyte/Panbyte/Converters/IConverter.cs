using Panbyte.Formats;

namespace Panbyte.Converters;

public interface IConverter
{
    Format InputFormat { get; }
    string ConvertTo(string value, Format outputFormat);
}