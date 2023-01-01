using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using reenbitChat.BLL.Hubs;
using reenbitChat.DAL.Context;

namespace reenbitChat.BLL.Services;

public abstract class ServiceBase
{
    protected readonly ChatContext _context;
    protected readonly IHubContext<ChatHub> _hub;
    protected readonly IMapper _mapper;

    protected ServiceBase(ChatContext context, IHubContext<ChatHub> hub, IMapper mapper)
    {
        _context = context;
        _hub = hub;
        _mapper = mapper;
    }
}
