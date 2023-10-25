using Application.DTOs.Request.Vacancy;
using Domain.Entities;

namespace Application.Mappers.VacancyProfile
{
	public class CreateOrUpdateVacancyProfile : Profiles
	{
		public CreateOrUpdateVacancyProfile()
		{
			CreateMap<UpdateVacancyRequestDto, Vacancy>() 
				.ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


			CreateMap<CreateVacancyRequestDto, Vacancy>()
				.ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
				.ForMember(dest => dest.CreationDate, opt => opt.Ignore());


		}


	}
}
