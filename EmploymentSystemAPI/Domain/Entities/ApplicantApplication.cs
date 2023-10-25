namespace Domain.Entities;

public sealed class ApplicantApplication : BaseDbEntity
{
	public DateTime ApplicationDate { get; set; }

	public int ApplicantId { get; set; }
	public Applicant Applicant { get; set; }
	public int VacancyId { get; set; }
	public Vacancy Vacancy { get; set; }
}
