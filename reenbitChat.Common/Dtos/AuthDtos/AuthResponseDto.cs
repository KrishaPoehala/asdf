namespace reenbitChat.Common.Dtos.AuthDtos;

public class AuthResponseDto
{
    public bool IsAuthSuccessfull { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
}
