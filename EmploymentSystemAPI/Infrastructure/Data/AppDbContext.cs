using Application.DTOs;
using Application.Interfaces.DbContext;
using Domain.Entities;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace Infrastructure.Data
{
	public class AppDbContext : IdentityDbContext<User, Role, int>, IApplicationDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		#region Sets

		public Task<IDbContextTransaction> Transaction => Database.BeginTransactionAsync();

		public DbSet<ApplicantApplication> Applications { get; set; }

		public DbSet<Vacancy> Vacancies { get; set; }
		public DbSet<Applicant> Applicants { get; set; }
		public DbSet<Employer> Employers { get; set; }

		public DbSet<Experience> Experiences { get; set; }


		#endregion
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			// Configure the User entity
			base.OnModelCreating(modelBuilder);

			 
			modelBuilder.Entity<User>().Property(u => u.CreationDate).HasDefaultValueSql("getdate()");
		
			 


			modelBuilder.Entity<Applicant>()
				.HasMany(a => a.Experience)
				.WithOne();

			modelBuilder.Entity<Applicant>()
				.HasMany(a => a.Applications)
				.WithOne();

			modelBuilder.Entity<Employer>()
				.HasOne(a => a.User)
				.WithOne();


		


			modelBuilder.Entity<Employer>()
				.HasOne(a => a.Department)
				.WithOne();

			modelBuilder.Entity<Vacancy>()
				.HasMany(v => v.Applications)
				.WithOne(a => a.Vacancy);

			modelBuilder.Entity<Vacancy>().Property(u => u.CreationDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<Vacancy>().Property(u => u.IsActive).HasDefaultValue(1);

			modelBuilder.Entity<ApplicantApplication>()
				.HasOne(a => a.Vacancy)
				.WithMany(v => v.Applications);

			modelBuilder.Entity<ApplicantApplication>()
				.HasOne(a => a.Applicant)
				.WithMany(v => v.Applications);

			modelBuilder.Entity<ApplicantApplication>().Property(u => u.CreationDate).HasDefaultValueSql("getdate()");


			new DbInitializer(modelBuilder).Seed();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await base.SaveChangesAsync(cancellationToken);
		}

	}
}
