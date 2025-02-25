using System.Text;

// Service for HTTP requests (GET and POST)
public class HttpService
{
    private readonly HttpClient _httpClient;

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Execution of GET request
    public async Task<string> GetDataAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // Execution of a POST request with content transfer
    public async Task<HttpResponseMessage> PostDataAsync(string url, HttpContent content)
    {
        var response = await _httpClient.PostAsync(url, content);
        return response;
    }

    // Overloaded POST method that accepts a JSON string
    public async Task<string> PostDataAsync(string url, string jsonContent)
    {
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}