using MarkDownTaking.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class GrammarController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApplicationDbContext _context;

    public GrammarController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    
    [HttpPost("check")]
    public async Task<IActionResult> CheckGrammar([FromBody] ContentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("Content cannot be empty.");
        }

        

        var parameters = new Dictionary<string, string>
        {
            { "text", request.Content },
            { "language", "en-US" }
        };

        var content = new FormUrlEncodedContent(parameters);

        var client = _httpClientFactory.CreateClient();


        var response = await client.PostAsync("https://api.languagetool.org/v2/check", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, $"Error: {errorResponse}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
       

        return Ok(responseContent);
    }


}

public class ContentRequest
{
    public string Content { get; set; }
}

public class GrammarResponse
{
    public List<GrammarIssue> Matches { get; set; }
}

public class GrammarIssue
{
    public string Message { get; set; }
    public string ShortMessage { get; set; }
    public string Replacements { get; set; }
    public string Offset { get; set; }
    public string Length { get; set; }
}
