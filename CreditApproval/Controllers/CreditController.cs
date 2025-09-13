namespace CreditApproval.Controllers;

[Route("api/[controller]/requests")]
[ApiController]
public class CreditController(
    IValidator<SubmitCreditDTO> creditValidator,
    ICreditService creditService) : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitCredit(SubmitCreditDTO data, CancellationToken token)
    {
        ValidationResult result = await creditValidator.ValidateAsync(data, token);

        if (!result.IsValid)
        {
            return BadRequest(JsonConvert.SerializeObject(result.Errors.Select(x => x.ErrorMessage)));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(JsonConvert.SerializeObject(ModelState.Values));
        }

        await creditService.SubmitCreditAsync(data, token);
        return StatusCode(201);
    }

    [HttpPatch("review")]
    public Task<IActionResult> ReviewRequest(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListRequests(string? creditType, string? status, CancellationToken token)
    {
        if (!string.IsNullOrWhiteSpace(creditType))
        {
            bool valid = Enum.TryParse<CreditType>(creditType, ignoreCase: true, out _);

            if (!valid)
            {
                return BadRequest("Invalid credit type filter");
            }
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            bool valid = Enum.TryParse<CreditStatus>(status, ignoreCase: true, out _);

            if (!valid)
            {
                return BadRequest("Invalid status filter");
            }
        }

        List<CreditDTO> credits = await creditService.GetAllAsync(creditType, status, token);

        return Ok(JsonConvert.SerializeObject(credits));
    }
}
