using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class ApplicantDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public UserDto User { get; set; }
		public string Education { get; set; }
		public string Skills { get; set; }
		public List<ExperienceDto> Experience { get; set; }
		public List<ApplicantApplicationDto> Applications { get; set; }

	}
}
