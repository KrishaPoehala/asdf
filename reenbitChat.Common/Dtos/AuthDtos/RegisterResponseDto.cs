namespace reenbitChat.Common.Dtos.AuthDtos;

public class RegisterResponseDto
{
    public string? Token { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public bool IsSuccessful { get; set; }
}
