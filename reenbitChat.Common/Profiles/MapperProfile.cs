using AutoMapper;
using reenbitChat.Common.Dtos.AuthDtos;
using reenbitChat.Common.Dtos.ChatDtos;
using reenbitChat.Common.Dtos.MessageDtos;
using reenbitChat.Common.Dtos.UserDtos;
using reenbitChat.DAL.Entities;

namespace reenbitChat.Common.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    { 
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();

        CreateMap<NewMessageDto, Message>();
        CreateMap<Message, MessageDto>().ReverseMap();
        CreateMap<Chat, ChatDto>();

        CreateMap<NewChatDto, Chat>();
        CreateMap<Chat, ChatDto>();

        CreateMap<RegisterUserDto, User>()
            .ForMember(x => x.NormalizedEmail, opt => opt.MapFrom(x => x.Email));
    }
}
