using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using reenbitChat.Common.Dtos.AuthDtos;
using reenbitChat.Common.Dtos.UserDtos;
using reenbitChat.DAL.Entities;
using reenbitChat.Domain.Jwt;
using System.Security.Claims;

namespace reenbitChat.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsCountroller: ControllerBase
{
    private readonly IJwtProvider _jwtProvider;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    public AccountsCountroller(IJwtProvider jwtProvider, UserManager<User> userManager, IMapper mapper)
    {
        _jwtProvider = jwtProvider;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
        {
            return Unauthorized(new AuthResponseDto() { ErrorMessage = "Email does not exist" });
        }

        var checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (checkPassword == false)
        {
            return Unauthorized(new AuthResponseDto() { ErrorMessage = "Invalid password" });
        }

        var claims = GetClaims(user);
        var token = _jwtProvider.GenerateToken(claims);
        return Ok(new AuthResponseDto() { IsAuthSuccessfull = true, Token = token });
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        if (dto is null || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var newUser = _mapper.Map<User>(dto);
        var result = await _userManager.CreateAsync(newUser, dto.Password);
        if (result.Succeeded == false)
        {
            var errors = result.Errors.Select(x => x.Description);
            return BadRequest(new RegisterResponseDto() { Errors = errors });
        }

        var claims = GetClaims(newUser);
        var token = _jwtProvider.GenerateToken(claims);
        return Ok(new RegisterResponseDto() { IsSuccessful = true, Token = token });
    }

    [NonAction]
    public List<Claim> GetClaims(User user) =>
        new()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.NormalizedEmail),
            new(JwtRegisteredClaimNames.Name, user.UserName),
        };
}
