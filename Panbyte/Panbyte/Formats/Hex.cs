namespace Panbyte.Formats;

/// <summary>
/// Hexadecimal format - bytes encoded as a hexadecimal string.
/// </summary>
public class Hex : IFormat
{
    public override string ToString()
    {
        return "hex";
    }
}