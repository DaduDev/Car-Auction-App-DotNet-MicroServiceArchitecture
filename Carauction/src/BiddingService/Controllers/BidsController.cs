using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GrpcAuctionClient _client;

    public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient client)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _client = client;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BidDto>> PlaceBid(string AuctionId, int Amount)
    {
        var auction = await DB.Find<Auction>().OneAsync(AuctionId);

        if(auction == null) {

            auction = _client.GetAuction(AuctionId);
            if(auction == null) return BadRequest("Can't accept bids at this moment");
        }
        
        if(auction.Seller == User.Identity.Name)
        {
            return BadRequest("Can't bid on your own car");
        }

        var bid = new Bid
        {
            Amount = Amount,
            AuctionId = AuctionId,
            Bidder = User.Identity.Name
        };

        if(auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        } else
        {
            var highBid = await DB.Find<Bid>()
            .Match(a => a.AuctionId == AuctionId)
            .Sort(b => b.Descending(x => x.Amount))
            .ExecuteFirstAsync();

            if(highBid != null && Amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = Amount > auction.ReservePrice
                    ? BidStatus.Accepted
                    : BidStatus.AcceptedBelowReserve;
            }

            if(highBid != null && bid.Amount <= highBid.Amount)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await DB.SaveAsync(bid);
        await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(bid));
        return Ok(_mapper.Map<BidDto>(bid));
    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(x => x.BidTime))
            .ExecuteAsync();
        return bids.Select(_mapper.Map<BidDto>).ToList();
    } 
}