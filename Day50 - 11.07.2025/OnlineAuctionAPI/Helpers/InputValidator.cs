namespace OnlineAuctionAPI.Helpers;

public static class InputValidator
{
    public static void ValidateString(string value, string pName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{pName} must not be null, empty or whitespace");
    }
}