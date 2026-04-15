using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using POS.Infrastructure.Services.Interfaces;

namespace POS.Infrastructure.Services
{
    public class GeminiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _modelId;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"] ?? "";
            _modelId = configuration["Gemini:ModelId"] ?? "gemini-2.0-flash";
        }

        public async Task<AiMenuSuggestionDto?> AnalyzeMenuItem(string? base64Image, string? menuName)
        {
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == "YOUR_GEMINI_API_KEY_HERE")
            {
                // Demo fallback with realistic ingredient data
                return new AiMenuSuggestionDto
                {
                    Name = !string.IsNullOrEmpty(menuName) ? menuName : "ผัดกะเพราหมูสับ",
                    NameEn = "Basil Stir-Fried Pork",
                    Description = "หมูสับผัดกะเพราจัดจ้าน หอมใบกะเพราสด เสิร์ฟพร้อมข้าวสวยร้อนๆ",
                    Price = 65,
                    Category = "อาหารจานหลัก",
                    Ingredients = new List<AiIngredientSuggestion>
                    {
                        new() { Name = "หมูสับ",       QuantityUsed = 150, Unit = "กรัม", EstimatedCostPerUnit = 0.12m },
                        new() { Name = "ใบกะเพรา",     QuantityUsed = 30,  Unit = "กรัม", EstimatedCostPerUnit = 0.05m },
                        new() { Name = "พริกขี้หนู",   QuantityUsed = 10,  Unit = "กรัม", EstimatedCostPerUnit = 0.08m },
                        new() { Name = "กระเทียม",     QuantityUsed = 15,  Unit = "กรัม", EstimatedCostPerUnit = 0.06m },
                        new() { Name = "น้ำมันพืช",    QuantityUsed = 20,  Unit = "มล.",  EstimatedCostPerUnit = 0.04m },
                        new() { Name = "น้ำปลา",       QuantityUsed = 15,  Unit = "มล.",  EstimatedCostPerUnit = 0.03m },
                        new() { Name = "ข้าวสวย",      QuantityUsed = 200, Unit = "กรัม", EstimatedCostPerUnit = 0.02m },
                    }
                };
            }

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelId}:generateContent?key={_apiKey}";

            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("Analyze this Thai food/dish information and provide details for a restaurant POS menu in JSON format.");
            
            if (!string.IsNullOrEmpty(menuName))
            {
                promptBuilder.AppendLine($"The menu name provided is: {menuName}");
            }
            
            if (!string.IsNullOrEmpty(base64Image))
            {
                promptBuilder.AppendLine("An image of the dish is also provided for visual analysis.");
            }

            promptBuilder.AppendLine(@"
Required JSON structure:
{
  ""Name"": ""ชื่อเมนูภาษาไทย"",
  ""NameEn"": ""English Name"",
  ""Description"": ""คำอธิบายเมนูภาษาไทย"",
  ""Price"": 0,
  ""Category"": ""Choose from: อาหารจานหลัก, เครื่องดื่ม, ของหวาน, เมนูพิเศษ"",
  ""Ingredients"": [
    {
      ""Name"": ""ชื่อวัตถุดิบ"",
      ""QuantityUsed"": 0,
      ""Unit"": ""กรัม/มล./ชิ้น/ฟอง/ลูก"",
      ""EstimatedCostPerUnit"": 0.00
    }
  ]
}

Rules:
- Price should be realistic Thai restaurant pricing (฿30-฿300 range)
- Ingredients should list ALL raw materials needed for ONE serving
- QuantityUsed = amount per single serving
- EstimatedCostPerUnit = cost per 1 unit (e.g., per gram, per ml) in Thai Baht
- Use metric units: กรัม for solids, มล. for liquids, ฟอง for eggs, ลูก for fruits
- If a menu name is provided, prioritize it for naming and ingredient selection.
- Return ONLY the JSON object, no extra text.");

            var parts = new List<object>();
            parts.Add(new { text = promptBuilder.ToString() });
            
            if (!string.IsNullOrEmpty(base64Image))
            {
                parts.Add(new { inline_data = new { mime_type = "image/jpeg", data = base64Image } });
            }

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = parts.ToArray()
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
                request.Headers.Add("User-Agent", "POS-Backend-App/1.0");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Gemini API Error (Status {response.StatusCode}): {errorBody}");
                }

                var result = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(result);
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrEmpty(text))
                    throw new Exception("Gemini API returned an empty response.");

                var jsonString = text.Replace("```json", "").Replace("```", "").Trim();

                var dto = JsonSerializer.Deserialize<AiMenuSuggestionDto>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (dto == null)
                    throw new Exception("Failed to deserialize AI suggestion from response.");

                return dto;
            }
            catch (HttpRequestException ex)
            {
                var message = ex.Message;
                if (ex.InnerException?.Message.Contains("software in your host machine") == true)
                {
                    message += ". IMPORTANT: This error indicates your Antivirus or Firewall is blocking the connection to Google Gemini.";
                }
                throw new Exception($"Gemini API Network/SSL Error: {message}. Inner: {ex.InnerException?.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Gemini API JSON Parsing Error: {ex.Message}", ex);
            }
        }
    }
}
