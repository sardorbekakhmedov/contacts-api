
using ContactsApi.Exceptions;

namespace ContactsApi.Middlewares;

public class CustomExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<CustomExceptionHandlingMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");

            var (statusCode, errorMessage) = ex switch
            {
                CustomNotFoundException notFound => (StatusCodes.Status404NotFound, notFound.Message),
                CustomConflictException conflict => (StatusCodes.Status409Conflict, conflict.Message),
                CustomBadRequestException badRequest => (StatusCodes.Status400BadRequest, badRequest.Message),
                _ => (StatusCodes.Status500InternalServerError, ex.Message)
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(errorMessage);
        }
    }
}