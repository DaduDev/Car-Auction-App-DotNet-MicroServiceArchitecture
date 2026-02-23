namespace AuctionService.DTOs;

public class UpdateAuctionDto
{
    public string Make { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int? Year { get; set; }
    public string Color { get; set; } = String.Empty;
    public int? Mileage { get; set; }
}