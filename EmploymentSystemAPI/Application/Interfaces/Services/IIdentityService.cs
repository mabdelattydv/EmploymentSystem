using Application.DTOs.Request.Identity;
using Application.DTOs.Response.Identity;

namespace Application.Interfaces.Services;

public interface IIdentityService
{
	Task<RegisterResponseDto> RegisterUser(RegisterRequestDto request);
	Task<LoginResponseDto> Login(LoginRequestDto request);
}
