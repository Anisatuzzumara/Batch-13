using System;
using AutoMapper;
using LibraryManagementMVC.DTOs;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.MappingProfiles;

public class PublisherMappingProfile : Profile
{
    public PublisherMappingProfile()
        {
            // Pemetaan dari entitas Publisher ke PublisherDto untuk tujuan tampilan.
            // Ini menghitung jumlah buku yang dimiliki oleh publisher.
            CreateMap<Publisher, PublisherDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PubId))
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count));
            
            // Pemetaan dari entitas Publisher ke DTO untuk mengisi form edit.
            CreateMap<Publisher, PublisherCreateUpdateDto>()
                .ForMember(dest => dest.PubId, opt => opt.MapFrom(src => src.PubId));

            // Pemetaan dari DTO form kembali ke entitas Publisher untuk proses penyimpanan.
            CreateMap<PublisherCreateUpdateDto, Publisher>()
                .ForMember(dest => dest.PubId, opt => opt.MapFrom(src => src.PubId));
        }
}
