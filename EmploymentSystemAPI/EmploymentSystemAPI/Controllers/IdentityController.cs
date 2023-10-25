using Application.Common.Enums;
using Application.Interfaces.Services;
using Resources;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Application.DTOs.Request.Identity;
using Application.DTOs.Response.Identity;
using Serilog;

namespace EmploymentSystemAPI.Controllers;

public class IdentityController : APIBaseController<IdentityController>
{
	private readonly IValidator<RegisterRequestDto> _registrationValidator;
	private readonly IValidator<LoginRequestDto> _loginValidator;
	private readonly IIdentityService _service;
	public IdentityController(IValidator<LoginRequestDto> loginValidator,
		IValidator<RegisterRequestDto> registrationValidator,
		IIdentityService service)
	{
		_loginValidator = loginValidator;
		_registrationValidator = registrationValidator;
		_service = service;
	}

	[HttpPost("Register")]
	public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto request)
	{
		RegisterResponseDto response = new()
		{
			Message = Resource.Failed,
			Code = ((byte)MessageEnum.ProcessFailed).ToString()
		};

		try
		{
			var validationResult = _registrationValidator.Validate(request);
			if (!validationResult.IsValid)
			{
				response.Code = ((byte)MessageEnum.ValidationError).ToString();
				response.Message = string.Join(",", SetErrorMessage(validationResult.Errors));
				return BadRequest(response);
			}

			response = await _service.RegisterUser(request);

		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}

		return Ok(response);
	}

	[HttpPost("login")]
	public async Task<ActionResult<RegisterResponseDto>> SignIn([FromBody] LoginRequestDto request)
	{
		LoginResponseDto response = new()
		{
			Message = Resource.Failed,
			Code = ((byte)MessageEnum.ProcessFailed).ToString()
		};

		try
		{
			var validationResult = _loginValidator.Validate(request);
			if (!validationResult.IsValid)
			{
				response.Code = ((byte)MessageEnum.ValidationError).ToString();
				response.Message = string.Join(",", SetErrorMessage(validationResult.Errors));
				return BadRequest(response);
			}

			response = await _service.Login(request);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, ex.Message, ex.StackTrace);
			response.Code = ((byte)MessageEnum.GeneralError).ToString();
			response.Message = Resource.GeneralError;
		}

		return Ok(response);
	}
}
