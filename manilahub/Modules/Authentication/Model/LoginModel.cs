using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Authentication.Model
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(j => j.Username)
                .NotEmpty()
                .MaximumLength(10)
                .MinimumLength(5)
                .Matches("^[a-zA-Z0-9]*$");
            RuleFor(j => j.Password)
                .NotEmpty();
        }
    }
}
