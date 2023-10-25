using Application.Utils;
using Resources;
using FluentValidation;
using Application.DTOs.Request.Identity;

namespace Application.DTOs.Request.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
	public LoginRequestValidator()
	{
		RuleFor(x => x.Email)
			.Matches(RegularExpressions.Email).WithMessage(x => $"{{PropertyName}}: {Resource.NotValidEmail}");
	}
}

