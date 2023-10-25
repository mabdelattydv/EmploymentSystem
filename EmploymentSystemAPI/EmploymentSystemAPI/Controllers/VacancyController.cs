using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Enums;
using Application.DTOs.Common;
using Application.DTOs.Request;
using Application.Interfaces.Services;
using Azure;
using Azure.Core;
using Resources;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Request.Identity;
using Application.DTOs.Request.Vacancy;
using Application.DTOs.Response.Applicant;

namespace EmploymentSystemAPI.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Employer")]
public class VacancyController : APIBaseController<VacancyController>
{
	private readonly IValidator<CreateVacancyRequestDto> _requestValidator;
	private readonly IValidator<DeactivateVacancyRequestDto> _deactivateValidator;
	private readonly IEmployerServices _service;
	public VacancyController(IValidator<LoginRequestDto> loginValidator,
		IValidator<CreateVacancyRequestDto> requestValidator,
		IValidator<DeactivateVacancyRequestDto> deactiveValidator,
		IEmployerServices service)
	{
		_requestValidator = requestValidator;
		_service = service;
		_deactivateValidator = deactiveValidator;
	}

	[HttpPost("PostVacancy")]
	public async Task<ActionResult<ResponseDto>> CreateNewVacancy(CreateVacancyRequestDto request)
	{
		var response = new ResponseDto()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.Failed
		};
		var actor = GetActor();
		if (!string.IsNullOrEmpty(actor))
		{
			request.CreatedBy = Convert.ToInt32(actor);
		}
		var validationResult = await _requestValidator.ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			response.Code = ((byte)MessageEnum.ValidationError).ToString();
			response.Message = string.Join(",", SetErrorMessage(validationResult.Errors));
			return BadRequest(response);
		}

		response = await _service.CreateVacancy(request);

		return Ok(response);
	}

	[HttpPut("UpdateVacancy/{vacancyId}")]
	public async Task<ActionResult<ResponseDto>> UpdateVacancy(int vacancyId, UpdateVacancyRequestDto request)
	{
		var response = new ResponseDto()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.Failed
		};
		var actor = GetActor();
		if (!string.IsNullOrEmpty(actor))
		{
			request.UpdatedBy = Convert.ToInt32(actor);
		}

		if (vacancyId <= 0)
		{
			response.Code = ((byte)MessageEnum.ValidationError).ToString();
			response.Message = " Vacancy Id is Required field";
			return BadRequest(response);
		}

		response = await _service.UpdateVacancy(vacancyId, request);

		return Ok(response);
	}

	[HttpPut("DeactivateVacancy/{vacancyId}")]
	public async Task<ActionResult<ResponseDto>> DeactivateVacancy(int vacancyId)
	{
		var response = new ResponseDto()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.Failed
		};
		DeactivateVacancyRequestDto request = new DeactivateVacancyRequestDto();
		request.VacancyId = vacancyId;
		var actor = GetActor();
		if (!string.IsNullOrEmpty(actor))
		{
			request.EmployerId = Convert.ToInt32(actor);
		}
		var validationResult = await _deactivateValidator.ValidateAsync(request);
		if (!validationResult.IsValid)
		{
			response.Code = ((byte)MessageEnum.ValidationError).ToString();
			response.Message = string.Join(",", SetErrorMessage(validationResult.Errors));
			return BadRequest(response);
		}

		response = await _service.DeactivateVacancy(request);

		return Ok(response);
	}


	[HttpGet("GetApplicantsForVacancy/{vacancyId}")]
	public async Task<ActionResult<ApplicantListResponseDTO>> GetAllApplicantsForVacancy(int vacancyId)
	{
		var response = new ApplicantListResponseDTO()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.Failed
		};

		if (vacancyId == 0)
		{
			response.Code = ((byte)MessageEnum.ValidationError).ToString();
			response.Message = " Vacancy Id is Required field";
			return BadRequest(response);
		}

		response = await _service.ViewVacancyApplicantList(vacancyId);

		return Ok(response);
	}
}