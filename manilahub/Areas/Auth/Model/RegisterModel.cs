using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Authentication.Model
{
    public class RegisterModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ContactNumber { get; set; }
        public int AgentId { get; set; }
        public double Balance { get; set; }
        public string ReferralCode { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }

    }

    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(j => j.Username)
                .NotEmpty()
                .MaximumLength(10)
                .MinimumLength(5)
                .Matches("^[a-zA-Z0-9]*$");
            RuleFor(j => j.Password)
                .Equal(j => j.ConfirmPassword)
                .WithMessage("Password did not match.")
                .NotEmpty();
            RuleFor(j => j.ConfirmPassword)
                .Equal(j => j.Password)
                .WithMessage("Password did not match.")
                .NotEmpty();
        }
    }
}
