using System;
using API.DTO;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDTO>()
            .ForMember(dest => dest.PhotoUrl,
                       opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain)!.Url))
            .ForMember(dest => dest.Age,
                       opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

        CreateMap<AppUser, UpdateMemberDTO>().ReverseMap();
        CreateMap<Photo, PhotoDTO>();

        CreateMap<Message, MessageDTO>()
            .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain)!.Url))
            .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain)!.Url))
            .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName))
            .ForMember(dest => dest.RecipientName, opt => opt.MapFrom(src => src.Recipient.UserName));
    }
}
