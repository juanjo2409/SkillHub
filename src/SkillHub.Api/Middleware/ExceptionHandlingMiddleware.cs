using System.Net;
using System.Text.Json;
using SkillHub.Domain.Exceptions;

namespace SkillHub.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            WorkshopNotFoundException or
            EnrollmentNotFoundException or
            InstructorNotFoundException or
            ParticipantNotFoundException => HttpStatusCode.NotFound,

            UnauthorizedEnrollmentAccessException => HttpStatusCode.Forbidden,

            DuplicateEnrollmentException or
            DuplicateWorkshopCodeException or
            WorkshopCapacityExceededException or
            WorkshopScheduleConflictException => HttpStatusCode.Conflict,

            DomainException => HttpStatusCode.BadRequest,

            _ => HttpStatusCode.InternalServerError
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled server error occurred.");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            error = statusCode.ToString(),
            message = statusCode == HttpStatusCode.InternalServerError
                ? "An unexpected error occurred on the server."
                : exception.Message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
