using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    private readonly IMapper _mapper;

    public AuctionDeletedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        var res = await DB.DeleteAsync<Item>(i => i.ID == context.Message.Id);

        if(!res.IsAcknowledged)
        {
            throw new Exception("Failed to delete item with id: " + context.Message.Id);
        }
    }
}