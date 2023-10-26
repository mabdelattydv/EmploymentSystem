using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Entities
{
	[Table("Users")]
	public class User : IdentityUser<int>
	{ 
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Address { get; set; } 
		public string? Role { get; set; }
		public DateTime CreationDate { get; set; }
		public int CreatedBy { get; set; } 
		public ICollection<ApplicantApplication> Applications { get; set; }
		public ICollection<Vacancy> Vacancies { get; set; }

	}
}
