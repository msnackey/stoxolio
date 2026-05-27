namespace Stoxolio.Service.BuildingBlocks.Common;

public record Error(string Code, string Message, ErrorType Type, string? UserMessage = null)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);
    public static readonly Error NullValue = new("General.Null", "Null value was provided", ErrorType.Failure);
    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
    public static Error Problem(string code, string message) => new(code, message, ErrorType.Problem);
    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
    public static Error BadGateway(string code, string message) => new(code, message, ErrorType.BadGateway);
}