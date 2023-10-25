using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.DTOs
{
	public class VacancyDto : BaseDbEntityDto
	{
		public string Number { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public DateTime ExpiryDate { get; set; }
		public bool IsActive { get; set; } = true;
		public int MaxNumberOfApplications { get; set; } 
		public List<ApplicantApplicationDto> Applications { get; set; }
	}
}
