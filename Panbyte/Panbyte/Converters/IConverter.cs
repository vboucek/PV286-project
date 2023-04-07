using Panbyte.Formats;

namespace Panbyte.Converters;

/// <summary>
/// Class for converting input from one format to another.
/// </summary>
public interface IConverter
{
    Format InputFormat { get; }
    
    /// <summary>
    /// Converts given value in a specified format.
    /// </summary>
    /// <param name="value">String value for conversion.</param>
    /// <param name="outputFormat">Output format object.</param>
    /// <returns>Converted string in specified format.</returns>
    string ConvertTo(string value, Format outputFormat);
}