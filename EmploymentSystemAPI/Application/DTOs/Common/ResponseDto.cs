using Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Common
{
	public class ResponseDto
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public ResponseDto() { }
		public ResponseDto(MessageEnum message)
		{
			Code = ((byte)message).ToString();
			Message = message.ToString();
		}
	}
}
