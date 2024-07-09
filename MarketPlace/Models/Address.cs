using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Please enter the streetName")]
        [MaxLength(200, ErrorMessage = "Maximum length of street name is 200 characters")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Please enter city name")]
        [MaxLength(100, ErrorMessage = "Maximum length of city name is 100 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter Province name")]
        [MaxLength(50, ErrorMessage = "Maximum length of province name is 50 characters")]
        public string Province { get; set; }


        [Required(ErrorMessage = "Please enter post code")]
        [MaxLength(5, ErrorMessage = "Maximum length of postal cod is 5 characters")]
        public string PostalCode { get; set; }

    }
}
