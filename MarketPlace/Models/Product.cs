using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public enum Status
    {
        Active,
        Inactive,
        pending,
        sold
    }
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
        
        [DisplayName("Product description")]
        public string Description { get; set; }

        [DisplayName("Category name")]
        public int CategoryID { get; set; }
        public  Status Status { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateSold { get; set; }
        public List<byte> Images {get;set;}
        public int Views { get; set; }

        [DisplayName("Name of seller")]
        public int UserId { get; set; }

        public User Seller { get; set; }

        public Category Category { get; set; }


    }
}
