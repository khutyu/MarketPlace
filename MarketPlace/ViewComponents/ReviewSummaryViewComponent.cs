using Microsoft.AspNetCore.Mvc;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;

namespace MarketPlace.ViewComponents.ReviewSummaryViewComponent
{
    public class ReviewSummaryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync( IEnumerable<Review> reviews)
        {
            int totalReviews = reviews.Count();
            if (totalReviews <= 0)
            {
                return View(new ReviewSummaryViewModel { 
                    AverageRating = 0,
                    TotalReviews = 0,
                    RatingBreakdown = new Dictionary<int, int>()
                });
            }
            int sum = 0;

            foreach (var review in reviews)
            {
                sum += review.Rating;
            }
            
            var model = new ReviewSummaryViewModel
            {
                AverageRating = sum/totalReviews,
                TotalReviews = totalReviews,
                RatingBreakdown = reviews.GroupBy(r => r.Rating)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(model);
        }
    }
    public class ReviewSummaryViewModel
    {
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingBreakdown { get; set; }
    }
}