using System;
using AutoMapper;

namespace webAPI
{
    public class HeroProfile : Profile
    {
        public HeroProfile()
        {
            CreateMap<SuperHeroDto, SuperHero>()
            .ForMember(
                dest => dest.Name,
                from => from.MapFrom(x => $"{x.Name}")
            )
            .ForMember(
                dest => dest.LastName,
                from => from.MapFrom(x => $"{x.LastName}")
            )
            .ForMember(
                dest => dest.FirstName,
                from => from.MapFrom(x => $"{x.FirstName}")
            );
            // .ForMember(
            //     dest => dest.Places,
            //     from => from.MapFrom(x => $"{x.Places}")
            // );

        }
    }
}