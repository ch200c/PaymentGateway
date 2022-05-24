using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult HandleError()
    {
        return Problem();
    }
}