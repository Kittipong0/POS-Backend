using System.Threading.Tasks;

namespace POS.Infrastructure.Services.Interfaces
{
    public interface IAiService
    {
        Task<AiMenuSuggestionDto?> AnalyzeMenuItem(string? base64Image, string? menuName);
    }

    public class AiMenuSuggestionDto
    {
        public string Name { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public List<AiIngredientSuggestion> Ingredients { get; set; } = new();
    }

    public class AiIngredientSuggestion
    {
        public string Name { get; set; } = string.Empty;
        public decimal QuantityUsed { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal EstimatedCostPerUnit { get; set; }
    }
}
