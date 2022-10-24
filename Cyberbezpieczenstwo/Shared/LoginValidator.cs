using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using FluentValidation;

namespace Cyberbezpieczenstwo.Shared;

public class LoginValidator : AbstractValidator<UserDTO>
{
	private PasswordLimitationController _limitationController { get; set; }
	
	public LoginValidator(bool first, bool second, bool third)
	{
		RuleFor(form => form.Password).NotEmpty().WithMessage("Hasło nie może być puste")
			.MinimumLength(first ? 8 : 0).WithMessage("Hasło musi składać się z co najmniej 8 znaków");

		if (second)
		RuleFor(form => form.Password).Matches(@"[A-Z]+").WithMessage("Hasło musi się składać z co najmniej jednej wielkiej litery");

		if (third)
        RuleFor(form => form.Password).Matches(@"[\!\?\*\.]+").WithMessage("Hasło musi zawierać co najmniej jeden znak specjalny");
	}
}

