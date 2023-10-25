using Application.DTOs.Request;
using Application.DTOs.Request.Identity;
using Application.DTOs.Request.Vacancy;
using Application.DTOs.Request.Validators;
using Application.Mappers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Application;

public static class ConfigureServices
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(Profiles));
		services.AddTransient<IValidator<RegisterRequestDto>, RegisterRequestValidator>();
		services.AddTransient<IValidator<LoginRequestDto>, LoginRequestValidator>();
		services.AddTransient<IValidator<CreateVacancyRequestDto>, VacancyRequestValidator>();
		services.AddTransient<IValidator<UpdateVacancyRequestDto>, VacancyRequestValidator>();
		services.AddTransient<IValidator<DeactivateVacancyRequestDto>, DeactivateRequestValidator>();
		return services;
	}
}
