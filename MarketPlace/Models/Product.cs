using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage ="Please enter the product name.")]
        [DisplayName("Product name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Please enter the product price.")]
        [DisplayName("Product price")]
        public decimal Price { get; set; }
        
        [DisplayName("Product descriptiom")]
        public string Description { get; set; }
  

        [DisplayName("Category name")]
        public int CategoryID { get; set; }

        [DisplayName("Name of seller")]
        public int UserId { get; set; }

        public User Seller { get; set; }

        public Category Category { get; set; }
       


    }
}
