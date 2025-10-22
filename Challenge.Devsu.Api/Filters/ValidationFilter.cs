using Challenge.Devsu.Core.ExceptionDomain;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Challenge.Devsu.Api.Filters
{
    public sealed class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(kv => kv.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new ValidationException("Datos de entrada inválidos.", errors);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
