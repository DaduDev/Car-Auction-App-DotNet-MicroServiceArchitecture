using AuctionService.Data;
using Grpc.Core;

namespace AuctionService;

public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext _dbContext;

    public GrpcAuctionService(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context)
    {
        var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(request.Id));

        if(auction == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "NotFound"));
        }

        var response = new GrpcAuctionResponse
        {
            Auction = new GrcpAuctionModel {
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id = auction.Id.ToString(),
                ReservePrice = auction.ReservePrice.ToString(),
                Seller = auction.Seller
            }
        };

        return response;
    }
}