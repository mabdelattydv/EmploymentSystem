using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Identity;

public class BaseIdentityRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
