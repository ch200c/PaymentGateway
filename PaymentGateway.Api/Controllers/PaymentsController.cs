using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.GetPaymentDetails;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.ProcessPayment;
using System.Net.Mime;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
[Route("[controller]")]
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

    [HttpGet("{PaymentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaymentDetailsResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentDetails([FromRoute] GetPaymentDetailsRequest request)
    {
        var result = await _paymentService.GetPaymentDetailsAsync(request);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
