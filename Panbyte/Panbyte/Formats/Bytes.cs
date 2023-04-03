namespace Panbyte.Formats;

/// <summary>
/// Raw bytes format - raw bytes as received/output by the program.
/// </summary>
public class Bytes : IFormat
{
    public override string ToString()
    {
        return "bytes";
    }
}