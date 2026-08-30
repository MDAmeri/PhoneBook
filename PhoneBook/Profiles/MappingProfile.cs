using AutoMapper;
using PhoneBook.DTOs;
using PhoneBook.Models;

namespace PhoneBook.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Contact → ContactDto (و برعکس)
            CreateMap<Contact, ContactDto>().ReverseMap();

            // CreateContactDto → Contact
            CreateMap<CreateContactDto, Contact>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // UpdateContactDto → Contact
            CreateMap<UpdateContactDto, Contact>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}