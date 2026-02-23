using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class CreateAuctionDto
{
    [Required]
    public string Make { get; set; } = String.Empty;    
    [Required]

    public string Model { get; set; } = String.Empty;    
    [Required]

    public int Year { get; set; }    
    [Required]

    public string Color { get; set; } = String.Empty;    
    [Required]

    public int Mileage { get; set; }    
    [Required]

    public string ImageUrl { get; set; } = String.Empty;    
    [Required]

    public int ReservedPrice { get; set; }    
    [Required]

    public DateTime AuctionEnd { get; set; }    

}