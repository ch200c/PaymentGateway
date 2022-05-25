namespace PaymentGateway.Domain.Utils;

public static class CardNumberPrivacyFilter
{
    private static readonly char DigitMask = 'X';
    private static readonly int UnmaskedDigitCount = 4;

    public static string Mask(string data)
    {
        var originalLength = data.Length;
        var mask = Enumerable.Repeat(DigitMask, originalLength - UnmaskedDigitCount);
        return new string(mask.Concat(data.TakeLast(4)).ToArray());
    }
}