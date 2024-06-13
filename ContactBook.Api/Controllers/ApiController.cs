using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactBook.Api.Controllers;

//ApiController to klasa macierzysta naszych pozosta³ych kontrolerów
//Jej zadanie to automatyczne przetwarzanie b³êdów na odpowiednie jak najbardziej rest api odpowiedzi
[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            //Jeœli lista b³êdów jest pusta, zwraca ogólny problem
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            //Jeœli wszystkie b³êdy s¹ typu walidacyjnego, zwraca problem walidacji
            ValidationProblem(errors);
        }

        //Else: zwraca pierwszy b³¹d jako problem
        return Problem(errors[0]);
    }

    //Otrzymuje error, zwraca odpowiadaj¹cy mu status
    protected IActionResult Problem(Error error)
    {
        //Mapuje typ b³êdu na odpowiedni status HTTP
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, detail: error.Description);
    }

    // Przetwarza listê b³êdów walidacji, zwraca odpowiedni result HTTP
    protected IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        // Dodaje ka¿dy b³¹d walidacyjny do s³ownika
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }
        //Zwraca elegancki result z list¹ b³êdów
        return ValidationProblem(modelStateDictionary);
    }
}