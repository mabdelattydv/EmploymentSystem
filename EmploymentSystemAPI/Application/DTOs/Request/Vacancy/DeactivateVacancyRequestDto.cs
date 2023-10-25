using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Vacancy
{
    public class DeactivateVacancyRequestDto
    {
        public int EmployerId { get; set; }
        public int VacancyId { get; set; }
    }
}
