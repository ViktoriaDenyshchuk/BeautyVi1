using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/ai")]
public class AIController : Controller 
{
    private readonly HttpClient _httpClient;
    private readonly BeautyViContext _context;
    private readonly string _ollamaUrl = "http://localhost:11434/api/generate";

    public AIController(HttpClient httpClient, [FromServices] BeautyViContext context)
    {
        _httpClient = httpClient;
        this._context = context;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] string userMessage)
    {
        var products = await _context.Products
    .Include(p => p.ProductIngredients) 
    .ThenInclude(pi => pi.Ingredient) 
    .Include(p => p.ProductAllergens) 
    .ThenInclude(pa => pa.Allergen) 
    .ToListAsync();

        var productsList = string.Join("\n", products.Select(p =>
        {
            var ingredients = p.ProductIngredients?
                .Select(pi => pi.Ingredient?.Name)
                .Where(name => name != null)
                .ToList();

            var ingredientsString = ingredients != null && ingredients.Any()
                ? string.Join(", ", ingredients)
                : "No ingredients";

            var allergens = p.ProductAllergens?
                .Select(pa => pa.Allergen?.Name)
                .Where(name => name != null)
                .ToList();

            var allergensString = allergens != null && allergens.Any()
                ? string.Join(", ", allergens)
                : "No allergens";

            return $"{p.Name}: {p.Description}: {p.Price}: {p.Category?.NameCategory}: {p.SuitableFor?.NameSuitableFor}: {p.EffectType?.NameEffectType}: Ingredients: {ingredientsString}: Allergens: {allergensString}";
        }));

        var aiPrompt = $"The user writes: {userMessage}\n\n" +
                "Here is a list of products. Recommend ONLY those products that meet the user's request. Do not recommend products that do not match the user's criteria:\n" +
                productsList + "\n\n" +
                "Do not mention any list or database. Be friendly and helpful to the user. If no products match the user's request, politely inform them and suggest alternatives if possible.";

        var requestBody = new
        {
            //model = "llama2",
            model = "mistral",
            prompt = aiPrompt,
            stream = false
        };

        var jsonRequest = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_ollamaUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
        var aiResponse = jsonResponse.GetProperty("response").GetString();

        return Ok(aiResponse);
    }
}