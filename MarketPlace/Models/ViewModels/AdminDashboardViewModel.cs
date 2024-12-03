namespace MarketPlace.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int DailyActiveUsers { get; set; }
        public int SuspendedAccounts { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    public class StatisticsViewModel
    {
        public int DailyActiveUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
    }
}