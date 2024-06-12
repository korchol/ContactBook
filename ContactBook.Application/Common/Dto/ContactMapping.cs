using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Domain.Entities;

namespace ContactBook.Application.MappingProfiles;

public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        // Mapowanie z Contact do ContactDto
        CreateMap<Contact, ContactDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategorySet.Category))
            .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.CategorySet.Subcategory));

        // Mapowanie z ContactDto do Contact
        CreateMap<ContactDto, Contact>()
            .ForMember(dest => dest.CategorySetId, opt => opt.Ignore()) // Ignoruj CategorySetId - powinno być obsługiwane osobno
            .ForMember(dest => dest.CategorySet, opt => opt.Ignore());  // Ignoruj CategorySet - powinno być obsługiwane osobno
    }
}
