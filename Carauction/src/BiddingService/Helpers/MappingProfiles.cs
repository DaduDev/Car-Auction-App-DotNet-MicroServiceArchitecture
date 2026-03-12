using AutoMapper;
using BiddingService.Dtos;
using BiddingService.Models;

namespace BiddingService.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Bid, BidDto>();
    }
}