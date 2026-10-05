using Microsoft.AspNetCore.Diagnostics;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Presentation.ExceptionHandlers
{
    public class ForbiddenExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ForbidenException)
                return false;

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(exception.Message, cancellationToken);
            return true;
        }
    }
}
