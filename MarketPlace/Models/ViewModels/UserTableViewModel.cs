namespace MarketPlace.Models.ViewModels
{
    public class UserTableViewModel
    {
        public List<User> Users { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalUsers / (double)PageSize);
    }
}