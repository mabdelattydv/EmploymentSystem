using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Applicant  
	{
        public int Id { get; set; }
        public int UserId { get; set; }
		public User User { get; set; }
		public string Education { get; set; }
		public string Skills { get; set; }
		public ICollection<Experience> Experience { get; set; }
		public ICollection<ApplicantApplication> Applications { get; set; }

	}
}
