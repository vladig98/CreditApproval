namespace CreditApproval.Controllers;

[Route("api/[controller]/requests")]
[ApiController]
public class CreditController : ControllerBase
{
    [HttpPost("submit")]
    public Task<IActionResult> SubmitCredit(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("review")]
    public Task<IActionResult> ReviewRequest(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    [HttpGet("list")]
    public Task<IActionResult> ListRequests(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
