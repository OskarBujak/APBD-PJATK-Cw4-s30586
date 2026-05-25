using System;
using System.ComponentModel.DataAnnotations;

public class PcCreateUpdateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name can be maximum of 50 characters.")]
    public string Name { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Must be greater than 0.01")]
    public float Weight { get; set; }

    [Required]
    [Range(0, 120, ErrorMessage = "Must be between 0 and 120.")]
    public int Warranty { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Can't be negative.")]
    public int Stock { get; set; }
}