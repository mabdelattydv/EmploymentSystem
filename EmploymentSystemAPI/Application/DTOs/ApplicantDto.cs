using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class ApplicantDto
	{ 
		public string Education { get; set; }
		public string Skills { get; set; }
		public List<ExperienceDto> Experience { get; set; } 

	}
}
