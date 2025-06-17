using System;
using AutoMapper;
using LibraryManagementMVC.Dtos;
using LibraryManagementMVC.Models;


namespace LibraryManagementMVC.MappingProfiles;

public class BookMappingProfile : Profile
{
    /// <summary>
    /// Profil AutoMapper untuk pemetaan yang berhubungan dengan Book.
    /// Ini menangani konversi antara entitas Book dan DTO yang sesuai.
    /// Perhatikan pemetaan untuk BookDto yang mengambil nama Publisher dari relasi
    /// untuk kemudahan penampilan di UI.
    /// </summary>
    
    public BookMappingProfile()
        {
            // Pemetaan dari entitas Book ke BookDto untuk tujuan tampilan.
            // Ini menyertakan nama Publisher untuk kenyamanan di UI.
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher != null ? src.Publisher.Name : "N/A"));

            // Pemetaan dari entitas Book ke DTO untuk mengisi form edit.
            CreateMap<Book, BookCreateUpdateDto>();
            
            // Pemetaan dari DTO form kembali ke entitas Book untuk proses penyimpanan.
            CreateMap<BookCreateUpdateDto, Book>();
        }
}
