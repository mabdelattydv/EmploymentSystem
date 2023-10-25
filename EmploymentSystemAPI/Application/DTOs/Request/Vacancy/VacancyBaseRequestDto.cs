using Newtonsoft.Json;

namespace Application.DTOs.Request.Vacancy;

public class VacancyBaseRequestDto
{
	public string Number { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Location { get; set; }
	public DateTime ExpiryDate { get; set; }
	public bool IsActive { get; set; }
	public int MaxNumberOfApplications { get; set; }
}
