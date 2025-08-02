using ContactsApi.Filters;

namespace ContactsApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddAutoFluentValidation(this IMvcBuilder builder)
    {
        return builder.AddMvcOptions(option =>
        {
            option.Filters.Add<AutoValidationFilter>(AutoValidationFilter.OrderLowerThanModelStateInvalidFilter);
        });
    }
}