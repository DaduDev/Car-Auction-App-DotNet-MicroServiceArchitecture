namespace Contracts;

public class AuctionUpdated
{
    public string Id { get; set; }
    public string Make { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int? Year { get; set; }
    public string Color { get; set; } = String.Empty;
    public int? Mileage { get; set; }
}