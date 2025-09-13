namespace CreditApproval.Controllers;

[Route("api/[controller]/requests")]
[ApiController]
public class CreditController(
    IValidator<SubmitCreditDTO> creditValidator,
    IValidator<ReviewCreditDTO> reviewValidator,
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
    public async Task<IActionResult> ReviewRequest(ReviewCreditDTO data, CancellationToken token)
    {
        ValidationResult result = await reviewValidator.ValidateAsync(data, token);

        if (!result.IsValid)
        {
            return BadRequest(JsonConvert.SerializeObject(result.Errors.Select(x => x.ErrorMessage)));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(JsonConvert.SerializeObject(ModelState.Values));
        }

        await creditService.ReviewCreditAsync(data, token);

        return StatusCode(204);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListRequests(string? creditType, string? status, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(JsonConvert.SerializeObject(ModelState.Values));
        }

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
