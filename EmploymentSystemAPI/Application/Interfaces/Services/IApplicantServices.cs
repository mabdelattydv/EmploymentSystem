using Application.DTOs.Common;
using Application.DTOs.Request.Vacancy;
using Application.DTOs.Response.Vacancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IApplicantServices
	{
		Task<VacanciesResponseDto> Search(VacancySearchRequestDto searchRequest);
		Task<ResponseDto> Apply(int applicantId, int vacancyId);
	}
}
