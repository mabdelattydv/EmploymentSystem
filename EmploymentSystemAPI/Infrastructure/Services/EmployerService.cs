using Application.Common.Enums;
using Application.DTOs;
using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Resources;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.DTOs.Request.Vacancy;
using Application.DTOs.Response.Applicant;
using Azure.Core;

namespace Infrastructure.Services;

public class EmployerService : BaseService<EmployerService>, IEmployerServices
{ 
	public EmployerService(IMapper mapper,
		ILogger<EmployerService> logger,
		AppDbContext context) : base(mapper, logger, context) { }

	public async Task<ResponseDto> CreateVacancy(CreateVacancyRequestDto vacancyRequest)
	{
		var response = new ResponseDto()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.Failed
		};

		try
		{

			var vacancy = Mapper.Map<Vacancy>(vacancyRequest);
			var result = await Context.Vacancies.AddAsync(vacancy);
			if (result.State != EntityState.Added)
			{
				Logger.LogError(result.State.ToString());
				return response;
			}
			await Context.SaveChangesAsync(default);
			response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
			response.Message = Resource.Sucess;
		}
		catch (Exception ex)
		{
			return LogException(ex);
		}
		return response;
	}

	public async Task<ResponseDto> UpdateVacancy(int vacancyId, UpdateVacancyRequestDto vacancy)
	{
		var response = new ResponseDto()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.Failed
		};
		try
		{
			var affectedRows = 0;
			var vacancyEntity = await Context.Vacancies.Where(v => v.Id == vacancyId).FirstOrDefaultAsync();
			if (vacancyEntity != null)
			{ 
				vacancyEntity.CreationDate = DateTime.Now;
				Mapper.Map(vacancy, vacancyEntity);
				affectedRows = await Context.SaveChangesAsync(default);
			}

			if (affectedRows != 0)
			{
				response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
				response.Message = Resource.Sucess;
			}
		}
		catch (Exception ex)
		{
			return LogException(ex);
		}


		return response;
	}

	public async Task<ResponseDto> DeactivateVacancy(DeactivateVacancyRequestDto request)
	{
		var response = new ResponseDto()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.Failed
		};
		try
		{
			var vacancyEntity = await Context.Vacancies.Where(v => v.Id == request.VacancyId).FirstOrDefaultAsync();
			if (vacancyEntity != null)
			{
				vacancyEntity.IsActive = false;
				vacancyEntity.UpdatedBy = request.EmployerId;
				vacancyEntity.UpdatedAt = DateTime.Now;

				var affectedRows = await Context.SaveChangesAsync(default);
				if (affectedRows != 0)
				{
					response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
					response.Message = Resource.Sucess;
				}

				return response;
			}

			response.Message = Resource.NotFound;
			response.Code = ((byte)MessageEnum.NotFound).ToString();

		}
		catch (Exception ex)
		{
			return LogException(ex);
		}
		return response;
	}

	//TODO: Make View to return the needed data with no hustle about the query
	public async Task<ApplicantListResponseDTO> ViewVacancyApplicantList(int vacancyId)
	{
		var response = new ApplicantListResponseDTO()
		{
			Code = ((byte)MessageEnum.NotFound).ToString(),
			Message = Resource.NotFound
		};

		try
		{
			var applicantList = await Context.Applications.Where(v => v.VacancyId == vacancyId).ToListAsync();

			if (!applicantList.Any())
			{
				response.Message = Resource.NotFound;
				response.Code = ((byte)MessageEnum.NotFound).ToString();
				return response;
			}

			response.Message = Resource.Sucess;
			response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
			response.Applicants = Mapper.Map<IEnumerable<ApplicantDto>>(applicantList);
		}
		catch (Exception ex)
		{
			return (ApplicantListResponseDTO)LogException(ex);
		}
		return response;
	}
}

