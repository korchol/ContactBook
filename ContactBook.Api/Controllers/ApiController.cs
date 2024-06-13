using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactBook.Api.Controllers;

//ApiController to klasa macierzysta naszych pozosta�ych kontroler�w
//Jej zadanie to automatyczne przetwarzanie b��d�w na odpowiednie jak najbardziej rest api odpowiedzi
[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            //Je�li lista b��d�w jest pusta, zwraca og�lny problem
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            //Je�li wszystkie b��dy s� typu walidacyjnego, zwraca problem walidacji
            ValidationProblem(errors);
        }

        //Else: zwraca pierwszy b��d jako problem
        return Problem(errors[0]);
    }

    //Otrzymuje error, zwraca odpowiadaj�cy mu status
    protected IActionResult Problem(Error error)
    {
        //Mapuje typ b��du na odpowiedni status HTTP
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, detail: error.Description);
    }

    // Przetwarza list� b��d�w walidacji, zwraca odpowiedni result HTTP
    protected IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        // Dodaje ka�dy b��d walidacyjny do s�ownika
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }
        //Zwraca elegancki result z list� b��d�w
        return ValidationProblem(modelStateDictionary);
    }
}