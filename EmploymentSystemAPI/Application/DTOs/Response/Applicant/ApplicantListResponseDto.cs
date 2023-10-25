using Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response.Applicant
{
    public class ApplicantListResponseDTO : ResponseDto
    {
        public IEnumerable<ApplicantDto>? Applicants { get; set; }
    }
}
