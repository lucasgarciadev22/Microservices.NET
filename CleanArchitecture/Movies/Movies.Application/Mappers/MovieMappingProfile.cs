using AutoMapper;
using Movies.Application.Commands;
using Movies.Application.Responses;
using Movies.Core.Entities;

namespace Movies.Application.Mappers;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<Movie, MovieResponse>().ReverseMap();
        CreateMap<Movie, CreateMovieCommand>().ReverseMap();
        CreateMap<UpdateMovieCommand, Movie>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.DirectorName, opt => opt.MapFrom(src => src.DirectorName))
            .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear))
            .ReverseMap();
    }
}
