using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuctionService.Entities;

[Table("Items")]
public class Item
{
    public Guid id { get; set; }
    public string Make { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = String.Empty;
    public int Mileage { get; set; }
    public string ImageUrl { get; set; } = String.Empty;
    [JsonIgnore]
    public Auction? Auction { get; set; }
    public Guid AuctionId { get; set; }
}