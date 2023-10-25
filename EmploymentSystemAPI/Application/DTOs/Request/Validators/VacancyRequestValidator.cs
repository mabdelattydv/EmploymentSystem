using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;
using Application.DTOs.Request.Vacancy;

namespace Application.DTOs.Request.Validators;

public class VacancyRequestValidator : AbstractValidator<VacancyBaseRequestDto>
{
	public VacancyRequestValidator()
	{
		RuleFor(x => x.Description).NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}");


		RuleFor(x => x.Title)
			.NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}");

	}
}