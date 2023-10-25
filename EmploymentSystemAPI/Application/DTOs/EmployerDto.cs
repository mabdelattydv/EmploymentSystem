using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
	public class EmployerDto
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public UserDto User { get; set; }
		public int DepartmentId { get; set; }
		public DepartmentDto Department { get; set; }
	}
}
