using Authentication.Core.DTOs;
using FluentValidation;
using System.Data;

namespace Authentication.API.Validations
{
	public class CrateUserDtoValidator:AbstractValidator<CreateUserDto>
	{
		public CrateUserDtoValidator()
		{
			RuleFor(x=>x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is wrong");
			RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
			RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
		}
	}
}
