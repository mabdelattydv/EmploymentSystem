using Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Request.Vacancy;

namespace Application.DTOs.Request.Validators;

public class DeactivateRequestValidator : AbstractValidator<DeactivateVacancyRequestDto>
{
	public DeactivateRequestValidator()
	{
		RuleFor(x => x.VacancyId).NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}")
			.NotEqual(0).WithMessage(x => $"{{PropertyName}}: {Resource.Required}");

	}
}