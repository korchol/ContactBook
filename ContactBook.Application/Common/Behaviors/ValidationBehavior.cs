using ErrorOr;
using FluentValidation;
using MediatR;

namespace ContactBook.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Jeśli nie istnieje walidator danej komendy, pomija walidacje
        if (_validator is null)
        {
            return await next();
        }

        // waliduje i zwraca wynik walidacji
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        // walidacja przebiegła nieprawidłowo - konwertujemy domyślne błędy walidacji na błędy z pakietu ErrorOr i zwracamy
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage));

            return (dynamic)errors;
        }

        //przechodzimy dalej w strumieniu zachowań (w naszym przypadku mamy tylko walidator i handler więc do handlera)
        return await next();
    }
}
