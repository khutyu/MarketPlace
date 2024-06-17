using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class Product
    {
        public int ProductId;

        [Required(ErrorMessage ="Please enter the product name.")]
        [DisplayName("Product name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter the product price.")]
        [DisplayName("Product price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter the number of items stocked.")]
        [DisplayName("Item stocked")]
        public int QuantityStocked { get; set; }


        public int QuantityBought { get; set; } = 0;    

        [DisplayName("Product name")]
        public string? Description { get; set; }

        [DisplayName("Product seller")]
        [Required(ErrorMessage ="Please enter the product seller")]
        public int SellerId { get; set; }


        [DisplayName("Category name")]
        [Required(ErrorMessage ="Please enter category name")]
        public int CategoryID { get; set; }

        public User Seller { get; set; }
        public Category Category { get; set; }


    }
}
