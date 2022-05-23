using OneOf;
using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Interfaces.Repositories;
using PaymentGateway.Application.ProcessPayment;
using System.Text.Json;

namespace PaymentGateway.Infrastructure;

public class PaymentService : IPaymentService
{
    private static readonly string DeserializationErrorMessage = "Deserialized value was null";

    private readonly IAcquiringBankClient _acquiringBankClient;
    private readonly ICardRepository _cardRepository;
    private readonly IPaymentDetailsRepository _paymentDetailsRepository;

    public PaymentService(
        IAcquiringBankClient acquiringBankClient, 
        ICardRepository cardRepository,
        IPaymentDetailsRepository paymentDetailsRepository)
    {
        _acquiringBankClient = acquiringBankClient;
        _cardRepository = cardRepository;
        _paymentDetailsRepository = paymentDetailsRepository;
    }

    public async Task<OneOf<ProcessPaymentSuccessfulResponse, ProcessPaymentUnsuccessfulResponse>> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        var card = await _cardRepository.UpsertCardAsync(request.CardNumber, request.ExpiryDate, request.Cvv);

        var serializedRequest = JsonSerializer.Serialize(request);

        var response = await _acquiringBankClient.ProcessPaymentAsync(serializedRequest);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var successfulResponse = JsonSerializer.Deserialize<ProcessPaymentSuccessfulResponse>(responseContent) ??
                throw new JsonException(DeserializationErrorMessage);

            await _paymentDetailsRepository.InsertPaymentDetailsAsync(
                successfulResponse.PaymentId, request.Amount, card, request.CurrencyCode, successfulResponse.Status);

            return successfulResponse;
        }
        else
        {
            return JsonSerializer.Deserialize<ProcessPaymentUnsuccessfulResponse>(responseContent) ?? 
                throw new JsonException(DeserializationErrorMessage);
        }
    }

    public async Task<GetPaymentDetailsResponse?> GetPaymentDetailsAsync(GetPaymentDetailsRequest request)
    {
        var paymentDetails = await _paymentDetailsRepository.GetPaymentDetailsAsync(request.PaymentId);

        if (paymentDetails == null)
        {
            return null;
        }

        var serializedRequest = JsonSerializer.Serialize(request);

        var response = await _acquiringBankClient.GetPaymentStatusAsync(serializedRequest);
        var responseContent = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        var statusResponse = JsonSerializer.Deserialize<GetPaymentStatusResponse>(responseContent) ?? 
            throw new JsonException(DeserializationErrorMessage);

        return new GetPaymentDetailsResponse(
            CardNumber: paymentDetails.Card.Number,
            ExpiryDate: paymentDetails.Card.ExpiryDate,
            Cvv: paymentDetails.Card.Cvv,
            Amount: paymentDetails.Amount,
            CurrencyCode: paymentDetails.CurrencyCode,
            Status: statusResponse.Status);
    }
}

