using System;
using AutoMapper;
using BrightTest.Models;
using BrightTest.ViewModels;

namespace BrightTest.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Request, RequestViewModel>()
                .ForMember(
                    options => options.DateTime,
                    configOptions => configOptions.MapFrom(req => $"{req.Date.ToShortDateString()} {req.Date.ToShortTimeString()}")
                );
        }
    }
}
