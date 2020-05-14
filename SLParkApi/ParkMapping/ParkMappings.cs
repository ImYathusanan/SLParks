using AutoMapper;
using SLParkApi.Models;
using SLParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLParkApi.ParkMapping
{
    public class ParkMappings : Profile
    {
        public ParkMappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrialDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
        }
    }
}
