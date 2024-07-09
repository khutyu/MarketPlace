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

        [DisplayName("Ratings")]
        public int Ratings { get; set; }

        
        [DisplayName("Product descriptiom")]
        public string Description { get; set; }
      
     
        [DisplayName("Name of seller")]
        [Required(ErrorMessage ="Please enter your name.")]
        public string SellerUserName { get; set; }

        [DisplayName("Comments")]
        public IEnumerable<Comment> Comments { get; set; }

        [DisplayName("Category name")]
        [Required(ErrorMessage = "Please enter category name")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }
       


    }
}
