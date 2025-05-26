using AutoMapper;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities.Command;

namespace PosTech.Contacts.ApplicationCore.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Contact, ContactResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Ddd, opt => opt.MapFrom(src => src.Ddd.DddCode))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));

            CreateMap<CreateAndUpdateContactRequestDto, CreateContactCommand>()
                .ForMember(dest => dest.Ddd, opt => opt.MapFrom(src => src.Ddd))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));

            CreateMap<CreateAndUpdateContactRequestDto, UpdateContactCommand>()
                .ForMember(dest => dest.Ddd, opt => opt.MapFrom(src => src.Ddd))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));

            CreateMap<SearchContactRequestDto, SearchContactsCommand>()
                .ForMember(dest => dest.DddCode, opt => opt.MapFrom(src => src.Ddd))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));
        }
    }
}
