namespace Application.DTOs;

public sealed class ApplicantApplicationDto : BaseDbEntityDto
{
	public DateTime ApplicationDate { get; set; }

	public int ApplicantId { get; set; }
	public ApplicantDto Applicant { get; set; }
	public int VacancyId { get; set; }
	public VacancyDto Vacancy { get; set; }
}
