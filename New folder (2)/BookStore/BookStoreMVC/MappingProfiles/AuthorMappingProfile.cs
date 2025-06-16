using System;
using AutoMapperBookStoreMVC.DTO;
using BookStoreMVC.DTO;
using BookStoreMVC.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreMVC.MappingProfiles;

public class AuthorMappingProfile : AutoMapper.Profile
{
    public AuthorMappingProfile()
    {
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.AuthorID, opt => opt.MapFrom(src => src.AuthorID));

        CreateMap<AuthorCreateDto, Author>()
            .ForMember(dest => dest.AuthorID, opt => opt.Ignore());
    }
}
