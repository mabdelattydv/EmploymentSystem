using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Enums
{
	public enum MessageEnum
	{
		ProcessedSuccessfully = 0,
		AuthenticationFailed = 1,
		ProcessFailed = 2,
		ValidationError = 3,
		Duplicated = 4,
		NotFound = 5,
		GeneralError = 99,
	}
}
