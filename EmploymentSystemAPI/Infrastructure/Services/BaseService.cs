using Application.Common.Enums;
using Application.DTOs.Common;
using Application.Interfaces.DbContext;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class BaseService<T> where T : class
{
	protected IMapper Mapper { get; }
	protected ILogger<T> Logger { get; }
	protected string SerializedResponse = string.Empty;
	protected IApplicationDbContext Context { get; set; }

	protected BaseService(IMapper mapper, ILogger<T> logger, IApplicationDbContext context
	)
	{
		Mapper = mapper;
		Logger = logger;
		Context = context;
	}

	protected ResponseDto LogException(Exception ex)
	{
		ResponseDto response = new ResponseDto(MessageEnum.GeneralError);
		Logger.LogError(ex.Message, ex.StackTrace);
		SerializedResponse = JsonConvert.SerializeObject(response);
		Logger.LogError(SerializedResponse);
		return response;
	}
}
