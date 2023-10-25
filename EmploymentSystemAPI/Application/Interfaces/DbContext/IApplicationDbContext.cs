using Application.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Interfaces.DbContext;

public interface IApplicationDbContext
{
	Task<IDbContextTransaction> Transaction { get; }
	DbSet<Vacancy> Vacancies { get; }
	DbSet<Applicant> Applicants { get; }
	DbSet<Employer> Employers { get; }
	DbSet<User> Users { get; }
	DbSet<Experience> Experiences { get; }
	DbSet<ApplicantApplication> Applications { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}


