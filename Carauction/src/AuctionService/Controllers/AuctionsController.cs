using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IPublishEndpoint publishEndpoint;

    public AuctionsController(AuctionDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
    {

        var query = dbContext.Auctions.OrderBy(x => x.Item!.Make).AsQueryable();

        if(!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);

        }

        return await query.ProjectTo<AuctionDto>(mapper.ConfigurationProvider).ToListAsync();
    } 

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await dbContext.Auctions.Include(x => x.Item).FirstOrDefaultAsync(c => c.Id == id);
        if(auction == null) return BadRequest();
        return Ok(mapper.Map<AuctionDto>(auction));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = mapper.Map<Auction>(auctionDto);

        auction.Seller = User.Identity.Name;

        dbContext.Auctions.Add(auction);

        var newAuction = mapper.Map<AuctionDto>(auction);

        await publishEndpoint.Publish(mapper.Map<AuctionCreated>(newAuction));

        var result = await dbContext.SaveChangesAsync() > 0;

        if(!result) return BadRequest();

        return CreatedAtAction(nameof(GetAuctionById), new{auction.Id}, newAuction);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<AuctionDto>> UpdateAuction(Guid id, [FromBody] UpdateAuctionDto auctionDto)
    {
        var auction = await dbContext.Auctions.Include(x => x.Item).FirstOrDefaultAsync(c => c.Id == id);

        if(auction == null) return NotFound();

        if(auction.Seller != User.Identity!.Name) return Forbid();

        auction.Item!.Make = auctionDto.Make ?? auction.Item.Make;
        auction.Item!.Model = auctionDto.Model ?? auction.Item.Model;
        auction.Item!.Color = auctionDto.Color ?? auction.Item.Color;
        auction.Item!.Mileage = auctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item!.Year = auctionDto.Year ?? auction.Item.Year;

        await publishEndpoint.Publish(mapper.Map<AuctionUpdated>(auction));

        var result = await dbContext.SaveChangesAsync() > 0;

        if(result)  return Ok(mapper.Map<AuctionDto>(auction));  

        return BadRequest();
    } 

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<AuctionDto>> DeleteAuction(Guid id)
    {
        var auction = await dbContext.Auctions.Include(x => x.Item).FirstOrDefaultAsync(c => c.Id == id);

        if(auction == null) return NotFound();
        
        if(auction.Seller != User.Identity!.Name) return Forbid();

        dbContext.Auctions.Remove(auction);

        await publishEndpoint.Publish<AuctionDeleted>(new {Id = auction.Id.ToString()});

        var res = await dbContext.SaveChangesAsync() > 0;

        if(!res)    return NotFound();

        return Ok(mapper.Map<AuctionDto>(auction));
    }
    
}