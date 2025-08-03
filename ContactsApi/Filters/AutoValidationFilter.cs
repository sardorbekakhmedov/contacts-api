
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactsApi.Filters;

public class AutoValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public static int OrderLowerThanModelStateInvalidFilter => -2001;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var paramter in context.ActionDescriptor.Parameters)
        {
            var isParametrFromBodyOrQuery =
                paramter.BindingInfo?.BindingSource == BindingSource.Body ||
                paramter.BindingInfo?.BindingSource == BindingSource.Query;

            var canBeValidated = isParametrFromBodyOrQuery && paramter.ParameterType.IsClass;
            var parametrGenericType = typeof(IValidator<>).MakeGenericType(paramter.ParameterType);

            if (canBeValidated && serviceProvider.GetService(parametrGenericType) is IValidator validator)
            {
                if (context.ActionArguments.TryGetValue(paramter.Name!, out var value))
                {
                    var validationContext = new ValidationContext<object?>(value);
                    var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);

                    if (!result.IsValid)
                        result.AddToModelState(context.ModelState, null);
                    
                }
            }

        }

        await next();    
    }
}