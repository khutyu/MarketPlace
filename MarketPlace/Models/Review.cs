using System.ComponentModel.DataAnnotations;

public class Review
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } //stores the Id of the user who owns the profile
    public string PosterId { get; set; } //Stores the Id of the user who posted the review 

    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }
}
