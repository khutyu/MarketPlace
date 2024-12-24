using MarketPlace.Shared.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
public class SearchViewModel
{
    // Search Parameters
    public string Query { get; set; }
    public List<int> SelectedCategories { get; set; } = new();
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string SortBy { get; set; }

    // Results
    public IEnumerable<Product> Products { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    public int TotalResults { get; set; }

    // Pagination
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);

    // Filter Options
    public List<SelectListItem> SortOptions => new()
    {
        new SelectListItem { Value = "relevance", Text = "Relevance" },
        new SelectListItem { Value = "price_asc", Text = "Price: Low to High" },
        new SelectListItem { Value = "price_desc", Text = "Price: High to Low" },
        new SelectListItem { Value = "newest", Text = "Newest First" }
    };

    // Helper Properties
    public bool HasResults => Products?.Any() ?? false;
    public bool HasFilters => SelectedCategories.Any() || MinPrice.HasValue || MaxPrice.HasValue;
    public string QueryDisplay => string.IsNullOrEmpty(Query) ? "All Products" : $"Results for '{Query}'";
}