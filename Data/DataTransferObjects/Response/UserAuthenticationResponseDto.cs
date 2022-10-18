namespace Data.Entities.DataTransferObjects.Response;

public class UserAuthenticationResponseDto
{
    public bool IsAuthSuccessful { get; set; }
    public string? Token { get; set; }
}