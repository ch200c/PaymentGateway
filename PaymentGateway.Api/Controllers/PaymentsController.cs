using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.ProcessPayment;
using System.Net.Mime;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProcessPaymentResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        var response = await _paymentService.ProcessPaymentAsync(request);

        return CreatedAtAction(nameof(GetPaymentDetails), new { PaymentId = response.PaymentId }, response);
    }

    [HttpGet("{PaymentId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaymentDetailsResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentDetails([FromRoute] GetPaymentDetailsRequest request)
    {
        var paymentDetails = await _paymentService.GetPaymentDetailsAsync(request);

        if (paymentDetails == null)
        {
            return NotFound();
        }

        return Ok(paymentDetails);
    }
}
