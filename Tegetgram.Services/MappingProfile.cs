using System;
using System.Linq;
using AutoMapper;
using Tegetgram.Data.Entities;
using Tegetgram.Services.DTOs;

namespace Tegetgram.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TegetgramUser, TegetgramUserDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages))
                .ForMember(dest => dest.BlockedUsers, opt => opt.MapFrom(src => src.BlockingUsers.Select(b => b.Blocked)));
            CreateMap<TegetgramUser, TegetgramUserItemDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Owner.UserName));
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Sender.Owner.UserName))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Recepient.Owner.UserName));
            CreateMap<Message, MessageItemDTO>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Sender.Owner.UserName))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Recepient.Owner.UserName));
        }
    }
}
