using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Interfaces.Repositories;
using PaymentGateway.Application.ProcessPayment;
using PaymentGateway.Domain.Utils;
using System.Text.Json;

namespace PaymentGateway.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private static readonly string DeserializationErrorMessage = "Deserialized value was null";

    private readonly IAcquiringBankClient _acquiringBankClient;
    private readonly ICardRepository _cardRepository;
    private readonly IPaymentDetailsRepository _paymentDetailsRepository;
    private readonly CardNumberPrivacyFilter _cardNumberPrivacyFilter;

    public PaymentService(
        IAcquiringBankClient acquiringBankClient,
        ICardRepository cardRepository,
        IPaymentDetailsRepository paymentDetailsRepository,
        CardNumberPrivacyFilter cardNumberPrivacyFilter)
    {
        _acquiringBankClient = acquiringBankClient;
        _cardRepository = cardRepository;
        _paymentDetailsRepository = paymentDetailsRepository;
        _cardNumberPrivacyFilter = cardNumberPrivacyFilter;
    }

    public async Task<ProcessPaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        var card = await _cardRepository.GetOrInsertAsync(request.CardNumber, request.CardExpiryDate, request.CardCvv);

        var serializedRequest = JsonSerializer.Serialize(request);

        var responseMessage = await _acquiringBankClient.ProcessPaymentAsync(serializedRequest);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        responseMessage.EnsureSuccessStatusCode();

        var deserializedResponse = JsonSerializer.Deserialize<ProcessPaymentResponse>(responseContent) ??
            throw new JsonException(DeserializationErrorMessage);

        await _paymentDetailsRepository.InsertAsync(
            deserializedResponse.PaymentId, request.Amount, card, request.CurrencyCode, deserializedResponse.Status);

        return deserializedResponse;
    }

    public async Task<GetPaymentDetailsResponse?> GetPaymentDetailsAsync(GetPaymentDetailsRequest request)
    {
        var paymentDetails = await _paymentDetailsRepository.GetAsync(request.PaymentId);

        if (paymentDetails == null)
        {
            return null;
        }

        var serializedRequest = JsonSerializer.Serialize(request);

        var responseMessage = await _acquiringBankClient.GetPaymentStatusAsync(serializedRequest);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        responseMessage.EnsureSuccessStatusCode();

        var deserializedResponse = JsonSerializer.Deserialize<GetPaymentStatusResponse>(responseContent) ??
            throw new JsonException(DeserializationErrorMessage);

        return new GetPaymentDetailsResponse(
            CardNumber: _cardNumberPrivacyFilter.Mask(paymentDetails.Card.Number, 4),
            CardExpiryDate: paymentDetails.Card.ExpiryDate,
            CardCvv: paymentDetails.Card.Cvv,
            Amount: paymentDetails.Amount,
            CurrencyCode: paymentDetails.CurrencyCode,
            Status: deserializedResponse.Status);
    }
}