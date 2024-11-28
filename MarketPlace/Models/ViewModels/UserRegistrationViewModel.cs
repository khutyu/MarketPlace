using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class UserRegistrationViewModel
{
    public string ReturnUrl { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }

    public string SecondName {get;set;}

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [Display(Name = "Profile Picture")]
    public IFormFile ProfilePicture { get; set; }

    // Address properties
    [Required(ErrorMessage = "Address Line 1 is required.")]
    [Display(Name = "Address Line 1")]
    public string AddressLine1 { get; set; }

    [Display(Name = "Address Line 2")]
    public string AddressLine2 { get; set; }

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required.")]
    [Display(Name = "State/Province")]
    public string State { get; set; }

    [Required(ErrorMessage = "Postal code is required.")]
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; }

    [Required(ErrorMessage = "You must agree to the terms and conditions.")]
    public bool IsAgreedToTerms { get; set; }

    // Optional fields can be included if necessary
    // public string SecurityQuestion { get; set; }
    // public string SecurityAnswer { get; set; }
}
