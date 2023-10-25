using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.DTOs.Request.Vacancy;
using Application.DTOs.Response.Applicant;

namespace Application.Interfaces.Services
{
    public interface IEmployerServices
	{
		Task<ResponseDto> CreateVacancy(CreateVacancyRequestDto vacancy);
		Task<ResponseDto> UpdateVacancy(int vacancyId, UpdateVacancyRequestDto vacancy);
		Task<ResponseDto> DeactivateVacancy(DeactivateVacancyRequestDto request);
		Task<ApplicantListResponseDTO> ViewVacancyApplicantList(int vacancyId);
	}
}
