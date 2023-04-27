using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // Way 1
                //var tasks = new List<Task>();
                //foreach (var validator in _validators)
                //{
                //    tasks.Add(Task.Run(() => validator.ValidateAsync(context, cancellationToken)));
                //}
                //await Task.WhenAll(tasks);

                // Way 2
                //var tasks2 = _validators.Select(validator => Task.Run(() => validator.ValidateAsync(context, cancellationToken))).ToList();
                //// Or
                ////var tasks2 = _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)).ToList();
                //await Task.WhenAll(tasks2);


                // Way 3
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Any())
                    throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
