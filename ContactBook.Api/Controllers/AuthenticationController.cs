using ContactBook.Application.Authentication.Commands;
using ContactBook.Application.Authentication.Common;
using ContactBook.Application.Authentication.Queries;
using ContactBook.Contracts.Authentication;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ContactBook.Api.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class AuthenticationController : ApiController
{
    private readonly IMediator _mediator;
    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //Tworzenie nowego u¿ytkownika
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.Email, request.Password);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => StatusCode(201, new { message = "User registered successfully." }),
            Problem);
    }

    //Logowanie u¿ytkownika i zwracanie tokenu JWT (lub niepowodzenia)
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);

        var authResult = await _mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == AuthenticationErrors.InvalidCredentials)
        {
            return Problem(
                detail: authResult.FirstError.Description,
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return authResult.Match(
            authResult => Ok(new AuthenticationResponse(authResult.Token)),
            Problem);
    }
}