using AutoMapper;
using ExchangeService.Application.DTO;
using ExchangeService.Domain.Models;

namespace ExchangeService.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ExchangeRate, ExchangeRateDTO>().ReverseMap();
        }
    }
}
