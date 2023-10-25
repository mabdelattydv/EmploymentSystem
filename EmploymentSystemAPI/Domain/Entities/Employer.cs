﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class Employer
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }
		public int DepartmentId { get; set; }
		public Department Department { get; set; }
	}
}
