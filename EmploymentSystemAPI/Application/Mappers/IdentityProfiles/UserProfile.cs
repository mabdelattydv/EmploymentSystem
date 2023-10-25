using Application.DTOs;
using Application.DTOs.Request.Identity;
using Application.Mappers;
using Domain.Entities;

namespace Application.Mappers.IdentityProfiles;

public class UserProfile : Profiles
{
	public UserProfile()
	{
		#region Register
		CreateMap<RegisterRequestDto, User>()
			.ForMember(dest => dest.UserName,
				opt => opt.MapFrom(src => src.Email));
		#endregion
	}
}
