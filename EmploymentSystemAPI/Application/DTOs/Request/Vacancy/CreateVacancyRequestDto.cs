using Newtonsoft.Json;

namespace Application.DTOs.Request.Vacancy;

public class CreateVacancyRequestDto : VacancyBaseRequestDto
{
 
	public int CreatedBy { get; set; }
	 
	public DateTime CreationDate { get; set; }
}