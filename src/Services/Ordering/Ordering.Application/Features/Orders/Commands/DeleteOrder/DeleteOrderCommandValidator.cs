using FluentValidation;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(p => p.Id)
               .LessThanOrEqualTo(0).WithMessage("{Id} must be a positive number.");
        }
    }
}
