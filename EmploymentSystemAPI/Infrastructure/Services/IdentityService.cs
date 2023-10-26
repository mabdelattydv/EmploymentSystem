using AutoMapper;
using Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Domain.Entities;
using Application.Interfaces.Services;
using Application.Interfaces.DbContext;
using Application.Common.Enums;
using Application.DTOs;
using Application.DTOs.Request.Identity;
using Application.DTOs.Response.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;


public class IdentityService : BaseService<IdentityService>, IIdentityService
{
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<Role> _roleManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Injecting Services needed to Register new user and Login 
	/// </summary>
	/// <param name="userManager"></param>
	/// <param name="signInManager"></param>
	/// <param name="roleManager"></param>
	/// <param name="mapper"></param>
	/// <param name="logger"></param>
	/// <param name="configuration"></param>
	/// <param name="context"></param>
	public IdentityService(UserManager<User> userManager,
		SignInManager<User> signInManager,
		RoleManager<Role> roleManager,
		IMapper mapper,
		ILogger<IdentityService> logger,
		IConfiguration configuration, IApplicationDbContext context
		) : base(mapper, logger, context)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_configuration = configuration;

		_roleManager = roleManager;
	}

	/// <summary>
	/// Endpoint for Login 
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	public async Task<LoginResponseDto> Login(LoginRequestDto request)
	{
		LoginResponseDto response = new()
		{
			Code = ((byte)MessageEnum.ProcessFailed).ToString(),
			Message = Resource.WrongData
		};
		try
		{
			User? user = await _userManager.FindByEmailAsync(request.Email);

			if (user == null)
			{
				response.Message = Resource.UserNotFound;
				response.Code = ((byte)MessageEnum.NotFound).ToString();
				return response;
			}
			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.Succeeded)
			{
				var token = await GenerateTokenAsync(user);
				response.ExpiresIn = 3600;
				response.AccessToken = token;
				response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
				response.Message = Resource.Sucess;
			}

		}
		catch (Exception ex)
		{
			return (LoginResponseDto)LogException(ex);
		}
		return response;
	}

	/// <summary>
	/// Endpoint for registering new User
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	public async Task<RegisterResponseDto> RegisterUser(RegisterRequestDto request)
	{


		return await RegisterUserAsync(request);

	}


	#region Utils
	private IEnumerable<string> SetErrorMessage(IEnumerable<IdentityError> errors)
	{
		foreach (var error in errors)
			yield return error.Description;
	}

	private async Task<string> GenerateTokenAsync(User? user)
	{

		var role = await _userManager.GetRolesAsync(user);

		var claims = new[]
		{
					new Claim(ClaimTypes.Email,user?.Email),
					new Claim(ClaimTypes.Actor, user.Id.ToString()),
					new Claim(ClaimTypes.Role, role?.FirstOrDefault())
				};

		SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
			_configuration["Jwt:Audience"],
			claims,
			expires: DateTime.Now.AddMinutes(60),
			signingCredentials: credentials);
		return new JwtSecurityTokenHandler().WriteToken(token);

	}


	private async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto request)
	{
		var user = Mapper.Map<User>(request);
		var role = request.Role.ToString();
		string password = request.Password.ToString();
		RegisterResponseDto response = new()
		{
			Message = Resource.Sucess,
			Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString()
		};
		await using var transaction = await Context.Transaction;
		try
		{
			var registered = await _userManager.CreateAsync(user, password);

			if (!registered.Succeeded)
			{
				await transaction.RollbackAsync();
				response.Message = "Error While Registering the user" +
										   string.Join(',', SetErrorMessage(registered.Errors));
				SerializedResponse = JsonConvert.SerializeObject(response);
				Logger.LogError(SerializedResponse);
				return response;
			}
			response = await AssignRoleAsync(user, role, transaction);

			if (registered.Succeeded)
			{
				if (request.Role == "Employer")
					response = await AddEmployerAsync(request, user, transaction);
				else
					response = await AddApplicantAsync(request, user, transaction);

				if (registered.Succeeded)
				{ 
					await transaction.CommitAsync();
					await Context.SaveChangesAsync(default);
				}
			}
			else
			{
				await transaction.RollbackAsync();
			}

			return response;

		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			return (RegisterResponseDto)LogException(ex);
		}
	}


	private async Task<RegisterResponseDto> AssignRoleAsync(User user, string role, IDbContextTransaction transaction)
	{
		RegisterResponseDto response = new()
		{
			Message = Resource.Sucess,
			Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString()
		};
		try
		{
			var roleExists = await _roleManager.RoleExistsAsync(role);
			if (!roleExists)
			{
				response.Code = ((byte)MessageEnum.ProcessFailed).ToString();
				response.Message = Resource.GeneralError;
				Logger.LogError("Role is not existing");
				return response;
			}
			await _userManager.AddToRoleAsync(user, role);
			response.Message = Resource.Sucess;
			response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
			 
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			return (RegisterResponseDto)LogException(ex);
		}
		return response;
	}

	private async Task<RegisterResponseDto> AddEmployerAsync(RegisterRequestDto request, User user, IDbContextTransaction transaction)
	{
		RegisterResponseDto response = new()
		{
			Message = Resource.Sucess,
			Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString()
		};
		try
		{
			var employer = Mapper.Map<Employer>(request);
			employer.User = user;
			employer.UserId = user.Id;

			var result = await Context.Employers.AddAsync(employer);
			if (result.State != EntityState.Added)
			{
				response.Code = ((byte)MessageEnum.ProcessFailed).ToString();
				response.Message = Resource.GeneralError; 
				return response;
			}

			response.Message = Resource.Sucess;
			response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
			 
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			return (RegisterResponseDto)LogException(ex);
		}
		return response;
	}

	private async Task<RegisterResponseDto> AddApplicantAsync(RegisterRequestDto request, User user, IDbContextTransaction transaction)
	{
		RegisterResponseDto response = new()
		{
			Message = Resource.Sucess,
			Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString()
		};
		try
		{
			var applicant = Mapper.Map<Applicant>(request);
			applicant.User = user;
			applicant.UserId = user.Id;
			var result = await Context.Applicants.AddAsync(applicant);
			if (result.State != EntityState.Added)
			{
				response.Code = ((byte)MessageEnum.ProcessFailed).ToString();
				response.Message = Resource.GeneralError; 
				return response;
			}

			response.Message = Resource.Sucess;
			response.Code = ((byte)MessageEnum.ProcessedSuccessfully).ToString();
			
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			return (RegisterResponseDto)LogException(ex);
		}
		return response;
	}


	#endregion
}
