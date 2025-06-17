using System;
using AutoMapper;
using LibraryManagementMVC.DTOs;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.Mapping;

public class PublisherMappingProfile : Profile
{
    public PublisherMappingProfile()
    {
        CreateMap<Publisher, PublisherDto>()
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count));
            
            CreateMap<Publisher, PublisherUpdateDto>();
            CreateMap<PublisherCreateDto, Publisher>();
            CreateMap<PublisherUpdateDto, Publisher>();
    }
}
