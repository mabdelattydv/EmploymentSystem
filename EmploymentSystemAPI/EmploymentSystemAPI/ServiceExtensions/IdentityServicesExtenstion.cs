using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace EmploymentSystemAPI.ServiceExtensions;

public static class IdentityServicesExtension
{
	public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddIdentity<User, Role>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();
		services.AddScoped<UserManager<User>>();
		services.AddAuthorization();
		return services;
	}
}
