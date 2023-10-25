using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Response.Vacancy;
using Application.Mappers;

namespace Application.Mappers.VacancyProfile
{
    public class VacancySearchResultProfile : Profiles
	{
		public VacancySearchResultProfile()
		{
			CreateMap<VacancyDto, VacancyResponseDto>().ReverseMap();
		}
	}
}
