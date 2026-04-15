namespace POS.Application.Models.Menu;

/// <summary>
/// Query parameters for paginated and filtered menu requests.
/// </summary>
public class MenuQueryRequest
{
    public string? SearchKeyword { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool OnlyRecommended { get; set; } = false;

    private int _page = 1;
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    private int _pageSize = 12;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 12 : (value > 50 ? 50 : value);
    }

    /// <summary>Allowed values: name | price | recommended</summary>
    public string SortBy { get; set; } = "recommended";
}
