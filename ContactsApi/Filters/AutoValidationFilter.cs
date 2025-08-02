
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
                var value = context.ActionArguments[paramter.Name];

                ValidationContext<object?> newValidation = new ValidationContext<object?>(value);
                CancellationToken requestAborted = context.HttpContext.RequestAborted;

                var result = await validator.ValidateAsync(newValidation, requestAborted);

                if (result.IsValid is false)
                    result.AddToModelState(context.ModelState, null);
            }

        }

        await next();    
    }
}