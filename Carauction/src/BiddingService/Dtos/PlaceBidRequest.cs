namespace BiddingService.Dtos;

public class PlaceBidRequest
{
    public string AuctionId { get; set; }
    public int Amount { get; set; }
}