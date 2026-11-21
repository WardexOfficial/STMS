using System.Text.Json;
using System.Text;

namespace stms;

public static class HttpHandler
{
    private static readonly HttpClient _httpClient = new HttpClient();
    public static async Task Handle(string targetUrl)
    {
        try
        {
            string jsonResponse = await _httpClient.GetStringAsync(targetUrl);

            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("url", out JsonElement urlElement) &&
                    root.TryGetProperty("count", out JsonElement countElement))
                {
                    string url = urlElement.GetString();
                    int count = countElement.GetInt32();

                    if (url != null)
                        for (int i = 0; i < count; i++)
                            await SendRequest(url);
                }
            }
        }
        catch 
        {
            throw;
        }
    }

    public static async Task SendRequest(string targetUrl)
    {
        try
        {
            string jsonPayload = "{\"world\": \"hello world\"}";
            using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(targetUrl, content);
        }
        catch (Exception)
        {
            return;
        }
    }
}