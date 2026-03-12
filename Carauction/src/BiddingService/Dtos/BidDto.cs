using BiddingService.Models;
using MongoDB.Entities;

namespace BiddingService.Dtos;

public class BidDto : Entity
{
    public string AuctionId {get; set;}
    public string Bidder { get; set; }
    public DateTime BidTime { get; set; } = DateTime.Now;
    public int Amount { get; set; }
    public BidStatus BidStatus { get; set; }
}