namespace POS.Application.Models;

/// <summary>
/// Generic wrapper for paginated results.
/// </summary>
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    /// <summary>Distinct categories found in the entire (unfiltered-by-category) dataset</summary>
    public IEnumerable<CategoryDto> AvailableCategories { get; set; } = Enumerable.Empty<CategoryDto>();
}
