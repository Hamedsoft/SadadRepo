using Application.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (CustomException ex)
        {
            httpContext.Response.StatusCode = 400;
            httpContext.Response.ContentType = "application/json";
            var errorResponse = new
            {
                Code = ex.ErrorCode,
                Message = ex.Message,
                Description = ex.Detail
            };
            await httpContext.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
