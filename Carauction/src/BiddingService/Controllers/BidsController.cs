using BiddingService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using BiddingService.Models;
using AutoMapper;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper _mapper;

    public BidsController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceBid([FromBody] PlaceBidRequest request)
    {
        var auction = await DB.Find<Auction>().OneAsync(request.AuctionId);

        //gPRC implementation

        if(auction == null)
        {
            return NotFound("Auction not found");
        }

        if(auction.Seller == User.Identity.Name)
        {
            return BadRequest("You cann't bid on your own auction");
        }

        var bid = new Bid
        {
            Amount = request.Amount,
            Bidder = User.Identity.Name,
            AuctionId = request.AuctionId,
        };

        if(auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        } else
        {
            var highBid = await DB.Find<Bid>()
                .Match(b => b.AuctionId == request.AuctionId)
                .Sort(b => b.Descending(bid => bid.Amount))
                .ExecuteFirstAsync();
            
            if(highBid != null && request.Amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = request.Amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
            } else
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await DB.SaveAsync(bid);

        return Ok(_mapper.Map<BidDto>(bid));
    }
}