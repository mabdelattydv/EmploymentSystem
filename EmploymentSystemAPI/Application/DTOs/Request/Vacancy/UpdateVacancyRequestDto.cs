using Newtonsoft.Json;

namespace Application.DTOs.Request.Vacancy;

public class UpdateVacancyRequestDto : VacancyBaseRequestDto
{

	public DateTime UpdatedAt { get; set; } = DateTime.Now;
	
	public int UpdatedBy { get; set; }
}
