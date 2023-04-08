namespace Panbyte.Utils;

public class GetMatchingBracket
{
    private static readonly Dictionary<byte, byte> BracketsPairs = new Dictionary<byte, byte>()
    {
        { Convert.ToByte('('), Convert.ToByte(')') },
        { Convert.ToByte('['), Convert.ToByte(']') },
        { Convert.ToByte('{'), Convert.ToByte('}') }
    };

    public static byte GetMatchingClosingBracket(byte openingBracket)
    {
        return BracketsPairs[openingBracket];
    }
}