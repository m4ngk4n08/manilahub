using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace manilahub.Modules.Transactions.Model
{
    public class TransactionModel
    {
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }
        public string Remarks { get; set; }
    }

    public class TransactionValidator : AbstractValidator<TransactionModel>
    {
        public TransactionValidator()
        {
            RuleFor(j => j.UserId).NotEmpty();
            RuleFor(j => j.Type).NotEmpty();
            RuleFor(j => j.Amount).NotEmpty();
        }
    }
}
