using Application.Interfaces.DbContext;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Utils.Extensions;

public static class AddDbServices
{
	public static IServiceCollection AddDbContextServices(this IServiceCollection services, string connectionString)
	{

		//Add DbContext using SQL server 

		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});

		services.AddScoped<IApplicationDbContext>(
			provider => provider.GetRequiredService<AppDbContext>());
		return services;
	}
}
