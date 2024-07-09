using MarketPlace.Models;
namespace MarketPlace
{
    public class Order
    {
        public int OrderId { get; set; }

        public string BuyerUserName { get; set; }
        public string SellerUserName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

       
        public IEnumerable<Product> Products { get; set; }
        
    }
}
