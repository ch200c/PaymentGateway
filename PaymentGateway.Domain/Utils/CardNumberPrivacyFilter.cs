namespace PaymentGateway.Domain.Utils;

public class CardNumberPrivacyFilter
{
    private readonly char _digitMask;

    public CardNumberPrivacyFilter(char digitMask)
    {
        _digitMask = digitMask;
    }

    public string Mask(string input, int unmaskedDigitCount)
    {
        var originalLength = input.Length;
        var mask = Enumerable.Repeat(_digitMask, originalLength - unmaskedDigitCount);

        return new string(
            mask
                .Concat(input.TakeLast(unmaskedDigitCount))
                .ToArray());
    }
}