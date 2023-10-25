using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Application.Interfaces.Services;
using Infrastructure.Services;
using Infrastructure.Utils.Extensions;

namespace Infrastructure
{
	public static class ConfigureServices
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			services.AddScoped<IIdentityService, IdentityService>();
			services.AddScoped<IEmployerServices, EmployerService>();
			services.AddScoped<IApplicantServices, ApplicantServices>();
			if (string.IsNullOrEmpty(connectionString))
			{
				Log.Logger.Error("Services Error: connection string is null, dbContext not set");
				return services;
			}
			services.AddDbContextServices(connectionString);

			return services;
		}
	}
}
