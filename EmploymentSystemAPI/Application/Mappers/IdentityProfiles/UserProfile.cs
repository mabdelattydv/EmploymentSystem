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


		CreateMap<RegisterRequestDto, Applicant>()
			.ForMember(dest => dest.Education,
				opt => opt.MapFrom(src => src.Applicant.Education))
			.ForMember(dest => dest.Skills,
				opt => opt.MapFrom(src => src.Applicant.Skills));


		CreateMap<RegisterRequestDto, Employer>()
			.ForMember(dest => dest.Department,
				opt => opt.MapFrom(src => src.Employer.Department));
		CreateMap<DepartmentDto, Department>()
			.ForMember(dest => dest.Name,
				opt => opt.MapFrom(src => src.Name));


		CreateMap<ExperienceDto, Experience>()
			.ForMember(dest => dest.StartDate,
				opt => opt.MapFrom(src => src.StartDate))
			.ForMember(dest => dest.StartDate,
				opt => opt.MapFrom(src => src.EndDate))
			.ForMember(dest => dest.StartDate,
				opt => opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.StartDate,
				opt => opt.MapFrom(src => src.CompanyName))
			.ForMember(dest => dest.StartDate,
				opt => opt.MapFrom(src => src.IsPresent));
		#endregion
	}
}
