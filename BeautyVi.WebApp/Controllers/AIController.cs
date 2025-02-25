using BeautyVi.Core.Entities;
using BeautyVi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Timers;

[Route("api/ai")]
public class AIController : Controller
{
    private readonly HttpService httpService;
    private readonly BeautyViContext context;
    private readonly IChatHistoryRepository chatHistoryRepository;
    private readonly UserManager<IdentityUser> userManager;
    private readonly string _ollamaUrl = "http://localhost:11434/api/generate";
    private readonly ILogger<AIController> _logger;

    public AIController(HttpService httpService, BeautyViContext context, IChatHistoryRepository chatHistoryRepository, UserManager<IdentityUser> userManager, ILogger<AIController> logger)
    {
        this.httpService = httpService;
        this.context = context;
        this.chatHistoryRepository = chatHistoryRepository;
        this.userManager = userManager;
        this._logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Get the user ID (if it doesn't exist, an error is thrown)
    private string GetValidatedUserId()
    {
        var userId = userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("User not found.");
        }
        return userId;
    }

    // Get the chat history for the user
    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory()
    {
        var userId = GetValidatedUserId();
        var chatHistory = chatHistoryRepository.GetAllByUserId(userId);
        if (!chatHistory.Any())
        {
            return Ok("No chat history found.");
        }
        return Ok(chatHistory);
    }

    // Main AI chat method
    [Authorize]
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] string userMessage)
    {
        // Validate the entered message
        if (string.IsNullOrEmpty(userMessage))
        {
            return BadRequest("User message cannot be empty.");
        }

        if (userMessage.Length > 500)
        {
            return BadRequest("Message is too long. Please limit it to 500 characters.");
        }

        var userId = GetValidatedUserId();

        // Check if the user exists in the database
        var userExists = context.Users.Any(u => u.Id == userId);
        if (!userExists)
        {
            return NotFound("User does not exist.");
        }

        // Get the last 5 messages for the context
        var previousMessages = chatHistoryRepository.GetAllByUserId(userId)
            .OrderByDescending(ch => ch.Timestamp)
            .Take(5)
            .Select(ch => new { ch.UserMessage, ch.AIResponse })
            .ToList();

        var contextMessages = string.Join("\n", previousMessages.Select(ch => $"User: {ch.UserMessage}\nAI: {ch.AIResponse}"));

        // Get all products from the database
        var products = await context.Products
            .Include(p => p.ProductIngredients)
            .ThenInclude(pi => pi.Ingredient)
            .Include(p => p.ProductAllergens)
            .ThenInclude(pa => pa.Allergen)
            .AsSplitQuery()
            .ToListAsync();

        // Generate a list of products as text for AI
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

        var aiPrompt = $"Here is the conversation history:\n{contextMessages}\n" +
               $"New User Message: {userMessage}\n\n" +
               "Here is a list of products. Recommend ONLY those products that meet the user's request. " +
               "Do not recommend products that do not match the user's criteria:\n" +
               $"{productsList}\n\n" +
               "Do not mention any list or database. Be friendly and helpful to the user. " +
               "Avoid repeating greetings like 'Hello' or 'Hi' in every response. " +
               "If no products match the user's request, politely inform them and suggest alternatives if possible.";

        var requestBody = new
        {
            //model = "llama2",
            model = "mistral",
            prompt = aiPrompt,
            stream = false
        };

        var jsonRequest = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpService.PostDataAsync(_ollamaUrl, content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
            var aiResponse = jsonResponse.GetProperty("response").GetString();

            // Save chat history
            chatHistoryRepository.Add(new ChatHistory { UserId = userId, UserMessage = userMessage, AIResponse = aiResponse, Timestamp = DateTime.UtcNow });

            return Ok(aiResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during AI request.");
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    // Clear chat history manually
    [Authorize]
    [HttpPost("history/clear")]
    public IActionResult ClearChatHistory()
    {
        try
        {
            var userId = GetValidatedUserId();
            chatHistoryRepository.DeleteAllByUserId(userId);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while clearing chat history.");
            return StatusCode(500, "An error occurred while clearing chat history.");
        }
    }
}

//Compressed code with implementation in the controller of deleting chats (automatically) and training the model
/*public class AIController : Controller 
{
    private readonly HttpClient httpClient;
    private readonly BeautyViContext context;
    private readonly IChatHistoryRepository chatHistoryRepository;
    private readonly UserManager<IdentityUser> userManager;
    private readonly string _ollamaUrl = "http://localhost:11434/api/generate";
    private readonly IServiceScopeFactory scopeFactory;
    private static System.Timers.Timer timer;
    private readonly ILogger<AIController> _logger;

    public AIController(HttpClient httpClient, [FromServices] BeautyViContext context, IChatHistoryRepository chatHistoryRepository, UserManager<IdentityUser> userManager, IServiceScopeFactory scopeFactory, ILogger<AIController> logger)
    {
        this.httpClient = httpClient;
        this.context = context;
        this.chatHistoryRepository = chatHistoryRepository;
        this.userManager = userManager;
        this.scopeFactory = scopeFactory;
        this._logger = logger;

        // Ініціалізація таймера для обох операцій (очищення старих повідомлень і тренування моделі)
        if (timer == null)
        {
            timer = new System.Timers.Timer(86400000); // 86400000 мс = 24 години
            timer.Elapsed += (sender, e) =>
            {
                Task.Run(() => CleanupOldMessages());
                Task.Run(() => TrainModel());
            };
            timer.AutoReset = true;
            timer.Enabled = true;
        }
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetChatHistory()
    {
        var userId = userManager.GetUserId(User);
        if (userId == null)
        {
            return NotFound("User not found.");
        }

        var chatHistory = chatHistoryRepository.GetAllByUserId(userId);
        return Ok(chatHistory);
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] string userMessage)
    {
        if (string.IsNullOrEmpty(userMessage))
        {
            return BadRequest("User message cannot be empty.");
        }

        var userId = userManager.GetUserId(User);

        if (userId == null)
        {
            return NotFound("User not found.");
        }

        var userExists = await context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return NotFound("User does not exist.");
        }

        var previousMessages = chatHistoryRepository.GetAllByUserId(userId)
            .OrderByDescending(ch => ch.Timestamp)
            .Take(5)
            .Select(ch => new { ch.UserMessage, ch.AIResponse })
            .ToList();

        var contextMessages = string.Join("\n", previousMessages.Select(ch => $"User: {ch.UserMessage}\nAI: {ch.AIResponse}"));

        var products = await context.Products
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

        var aiPrompt = $"Here is the conversation history:\n{contextMessages}\n" +
               $"New User Message: {userMessage}\n\n" +
               "Here is a list of products. Recommend ONLY those products that meet the user's request. " +
               "Do not recommend products that do not match the user's criteria:\n" +
               $"{productsList}\n\n" +
               "Do not mention any list or database. Be friendly and helpful to the user. " +
               "Avoid repeating greetings like 'Hello' or 'Hi' in every response. " +
               "If no products match the user's request, politely inform them and suggest alternatives if possible.";

        var requestBody = new
        {
            //model = "llama2",
            model = "mistral",
            prompt = aiPrompt,
            stream = false
        };

        var jsonRequest = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(_ollamaUrl, content);
            response.EnsureSuccessStatusCode(); // кине виняток, якщо статус-код не 2xx
            var responseString = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
            var aiResponse = jsonResponse.GetProperty("response").GetString();

            var chatHistory = new ChatHistory
            {
                UserId = userId, 
                UserMessage = userMessage,
                AIResponse = aiResponse,
                Timestamp = DateTime.UtcNow
                //Timestamp = DateTime.Now
            };

            chatHistoryRepository.Add(chatHistory);

            return Ok(aiResponse);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error during AI request.");
            return StatusCode(500, "An error occurred while communicating with the AI service.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during AI request.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("history/clear")]
    public IActionResult ClearChatHistory()
    {
        var userId = userManager.GetUserId(User);
        if (userId == null)
        {
            return NotFound("User not found.");
        }

        var userChatHistory = chatHistoryRepository.GetAllByUserId(userId);
        foreach (var message in userChatHistory)
        {
            chatHistoryRepository.Delete(message);
        }

        return RedirectToAction("Index"); 
    }

    [HttpDelete("history/cleanup")]
    private void CleanupOldMessages()
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BeautyViContext>();
            var chatHistoryRepository = scope.ServiceProvider.GetRequiredService<IChatHistoryRepository>();

            //var cutoffDate = DateTime.Now.AddMinutes(-1);// Очищення повідомлень старіших за 1 хв
            var cutoffDate = DateTime.Now.AddDays(-30); // Видалення повідомлень старіших ніж 30 днів
            var oldMessages = context.ChatHistories
                                     .Where(ch => ch.Timestamp < cutoffDate)
                                     .ToList();

            if (oldMessages.Any())
            {
                context.ChatHistories.RemoveRange(oldMessages);
                context.SaveChanges();
                //Console.WriteLine($"Deleted {oldMessages.Count} old messages.");
                _logger.LogInformation($"Deleted {oldMessages.Count} old messages.");
            }
            else
            {
                Console.WriteLine("No old messages found.");
            }
        }
    }

    // Тренування моделі
    private async Task TrainModel()
    {
        // Отримуємо всі чати для навчання
        var allChats = context.ChatHistories.ToList();

        var trainingData = allChats.Select(ch => new
        {
            prompt = ch.UserMessage,
            response = ch.AIResponse
        }).ToList();

        // Формуємо запит на навчання
        var aiTrainingRequest = new
        {
            model = "mistral",
            data = trainingData  // Навчальні дані на основі всіх чатів
        };

        var jsonTrainingRequest = JsonSerializer.Serialize(aiTrainingRequest);
        var content = new StringContent(jsonTrainingRequest, Encoding.UTF8, "application/json");

        var response = httpClient.PostAsync(_ollamaUrl + "/train", content).GetAwaiter().GetResult();
        var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        // Логіка для обробки відповіді
        Console.WriteLine(responseString);
    }
}*/
