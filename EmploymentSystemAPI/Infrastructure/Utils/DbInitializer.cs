using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Lookups;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Utils;

public class DbInitializer
{
	private readonly ModelBuilder _modelBuilder;

	public DbInitializer(ModelBuilder modelBuilder)
	{
		_modelBuilder = modelBuilder;
	}

	/// <summary>
	/// Seed Roles from User Type Enum to AspNetRoles Table in db
	/// </summary>
	public void Seed()
	{
		var count = 0;
		foreach (var role in Enum.GetValues(typeof(UserType)))
		{
			_modelBuilder.Entity<Role>().HasData(
				new Role()
				{
					Id = ++count,
					Name = role.ToString(),
					NormalizedName = role.ToString().ToUpper(),
					ConcurrencyStamp = Guid.NewGuid().ToString()
				}
			);
		}
	}
}