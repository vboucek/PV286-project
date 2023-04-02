namespace Panbyte.Utils;

public class GetMatchingBracket
{
    private static readonly Dictionary<char, char> _bracketsPairs = new Dictionary<char, char>()
    {
        { '(', ')' },
        { '[', ']' },
        { '{', '}' }
    };

    public static char GetMatchingClosingBracket(char openingBracket)
    {
        return _bracketsPairs[openingBracket];
    }
}