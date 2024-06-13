using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Domain.Entities;

namespace ContactBook.Application.MappingProfiles;

public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        CreateMap<Contact, ContactDto>()
            // Mapowanie właściwości CategoryName z właściwości CategorySet.Category
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategorySet.Category))
            // Mapowanie właściwości SubcategoryName z właściwości CategorySet.Subcategory
            .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.CategorySet.Subcategory));

        CreateMap<ContactDto, Contact>()
            .ForMember(dest => dest.CategorySetId, opt => opt.Ignore())
            .ForMember(dest => dest.CategorySet, opt => opt.Ignore());
    }
}
