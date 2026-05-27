using Microsoft.AspNetCore.Http.HttpResults;
using FluentValidation.Results;

namespace Stoxolio.Service.BuildingBlocks.Common;

public static class ApiResults
{
    public static ValidationProblem ValidationProblem(IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = validationFailures
            .GroupBy(f => string.IsNullOrWhiteSpace(f.PropertyName) ? string.Empty : f.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).Distinct().ToArray());

        return TypedResults.ValidationProblem(
            errors,
            title: "General.Validation",
            type: GetTypeUri(ErrorType.Validation),
            extensions: CreateExtensions(Error.Validation("General.Validation",
                "One or more validation errors occurred.")));
    }

    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return result.Error is ValidationError validationError
            ? CreateValidationProblem(validationError)
            : Results.Problem(
                title: GetTitle(result.Error),
                detail: GetDetail(result.Error),
                type: GetTypeUri(result.Error.Type),
                statusCode: GetStatusCode(result.Error.Type),
                extensions: CreateExtensions(result.Error));
    }

    private static string GetTitle(Error error) =>
        error.Type switch
        {
            ErrorType.Validation or ErrorType.Problem or ErrorType.NotFound or ErrorType.Conflict
                or ErrorType.BadGateway => error.Code,
            _ => "Server failure"
        };

    private static string GetDetail(Error error) =>
        error.Type switch
        {
            ErrorType.Validation or ErrorType.Problem or ErrorType.NotFound or ErrorType.Conflict
                or ErrorType.BadGateway => error.Message,
            _ => "An unexpected error occurred"
        };

    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Problem => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.BadGateway => StatusCodes.Status502BadGateway,
            _ => StatusCodes.Status500InternalServerError
        };

    private static ValidationProblem CreateValidationProblem(ValidationError validationError)
    {
        var errors = validationError.Errors
            .GroupBy(error => string.IsNullOrWhiteSpace(error.Code) ? string.Empty : error.Code)
            .ToDictionary(g => g.Key, g => g.Select(error => error.Message).Distinct().ToArray());

        return TypedResults.ValidationProblem(
            errors,
            title: "One or more validation errors occurred.",
            type: GetTypeUri(ErrorType.Validation),
            extensions: CreateExtensions(validationError));
    }

    public static string GetTypeUri(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            ErrorType.BadGateway => "https://tools.ietf.org/html/rfc7231#section-6.6.3",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

    private static Dictionary<string, object?> CreateExtensions(Error error)
    {
        var extensions = new Dictionary<string, object?>
        {
            ["errorCode"] = error.Code
        };

        if (!string.IsNullOrWhiteSpace(error.UserMessage))
        {
            extensions["userMessage"] = error.UserMessage;
        }

        return extensions;
    }
}