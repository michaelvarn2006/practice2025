namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        input = input.ToLower();
        char[] array = input.ToCharArray();
        var cleaned = from ch in array
                      where !(Char.IsPunctuation(ch) || Char.IsWhiteSpace(ch))
                      select ch;
        char[] cleanedarr = cleaned.ToArray();
        char[] reversed = (char[])cleanedarr.Clone();
        Array.Reverse(reversed);
        return new string(reversed) == new string(cleanedarr) && cleanedarr.Length > 0;
    }
}
