using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidation : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidation()
        {
            RuleFor(a => a.Username)
                .NotEmpty().WithMessage("{Username} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{Username} must not exceed 50 characters");

            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("{Email} is required");

            RuleFor(a => a.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than 0");
        }
    }
}
