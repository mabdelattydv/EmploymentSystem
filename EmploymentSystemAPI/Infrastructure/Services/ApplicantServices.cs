using System.Data;
using System.Diagnostics;
using Application.Common.Enums;
using Application.DTOs.Common;
using Application.Interfaces.DbContext;
using Application.Interfaces.Services;
using AutoMapper;
using Azure;
using Azure.Core;
using Resources;
using Infrastructure.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Application.DTOs.Request.Vacancy;
using Application.DTOs.Response.Vacancy;
using Domain.Entities;
// ReSharper disable All

namespace Infrastructure.Services;

public class ApplicantServices : BaseService<ApplicantServices>, IApplicantServices
{


	public ApplicantServices(IMapper mapper,
		ILogger<ApplicantServices> logger,
		IApplicationDbContext context) : base(mapper, logger, context)
	{
	}

	public async Task<VacanciesResponseDto> Search(VacancySearchRequestDto searchRequest)
	{
		var response = new VacanciesResponseDto()
		{
			Message = Resource.NotFound,
			Code = ((byte)MessageEnum.NotFound).ToString()
		};

		try
		{
			var vacanciesList = await Context.Vacancies
				.Where(x => x.IsActive && x.ExpiryDate > DateTime.Today)
				.WhereIf(!string.IsNullOrEmpty(searchRequest.Title) && !string.IsNullOrWhiteSpace(searchRequest.Title),
					x => x.Title.Equals(searchRequest.Title))
				.WhereIf(!string.IsNullOrEmpty(searchRequest.Number) && !string.IsNullOrWhiteSpace(searchRequest.Title),
					x => x.Number.Equals(searchRequest.Number))
				.ToListAsync();

			if (vacanciesList.Any())
			{
				response.Message = Resource.Sucess;
				response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();

				response.Vacancies = Mapper.Map<IEnumerable<VacancyResponseDto>>(vacanciesList);
			}
			return response;
		}
		catch (Exception ex)
		{
			return (VacanciesResponseDto)LogException(ex);
		}
	}

	public async Task<ResponseDto> Apply(int userId, int vacancyId)
	{
		var response = new ResponseDto()
		{
			Message = Resource.Failed,
			Code = ((byte)MessageEnum.ProcessFailed).ToString()
		};
		await using var transaction = await Context.Transaction;
		try
		{

			Applicant applicant = Context.Applicants.FirstOrDefault(ap => ap.UserId == userId);

			if (!HasAppliedToday(applicant.Id))
			{
				var application = new ApplicantApplication { VacancyId = vacancyId, ApplicantId = applicant.Id };
				var added = Context.Applications.Add(application);
				if (added.State != EntityState.Added)
				{
					await transaction.RollbackAsync();
					return response;
				}

				await transaction.CommitAsync();
				await Context.SaveChangesAsync(default);
				response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
				response.Message = Resource.Sucess;
				return response;
			}

			response.Message = Resource.YouShouldApplayAfter24Hours;
			response.Code = ((byte)MessageEnum.Duplicated).ToString();
		}

		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			return LogException(ex);
		}

		return response;
	}


	#region Utils

	private bool HasAppliedToday(int applicantId)
	{
		var hasAppliedToday = Context.Applications
			.Where(ap => ap.ApplicantId == applicantId)
			.Any(x => x.ApplicationDate.Date < DateTime.Now.Date);

		return hasAppliedToday;
	}
	#endregion

}