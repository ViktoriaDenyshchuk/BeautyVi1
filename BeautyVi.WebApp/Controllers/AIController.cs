using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/ai")]
public class AIController : Controller // Використовуємо Controller замість ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _ollamaUrl = "http://localhost:11434/api/generate";

    public AIController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] string userMessage)
    {
        // Формуємо запит до AI
        var aiPrompt = $"Користувач пише: {userMessage}\n\n" +
                       "Відповідай українською мовою, використовуючи чітку і природну мову. " +
                       "Будь дружелюбним і допоможи користувачу.";

        var requestBody = new
        {
            model = "llama2",
            prompt = aiPrompt,
            stream = false
        };

        var jsonRequest = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Відправляємо запит до Llama2
        var response = await _httpClient.PostAsync(_ollamaUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        // Парсимо відповідь і витягуємо лише текст
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
        var aiResponse = jsonResponse.GetProperty("response").GetString();

        return Ok(aiResponse); // Повертаємо відповідь у форматі JSON
    }
}