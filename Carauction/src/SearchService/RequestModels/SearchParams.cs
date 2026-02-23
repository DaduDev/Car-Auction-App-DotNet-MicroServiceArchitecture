namespace SearchService;
using System.ComponentModel.DataAnnotations;

public class SearchParams
{
    public string searchTerm { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be >= 1")]
    public int PageNumber { get; set; } = 1;
    [Range(1, 200, ErrorMessage = "PageSize must be between 1 and 200")]
    public int PageSize { get; set; } = 20;
    public string Seller { get; set; }
    public string Winner { get; set; }
    public string OrderBy { get; set; }
    public string FilterBy { get; set; }
}