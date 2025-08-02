using ContactsApi.Middlewares;
using System.Text.Json.Serialization;
using ContactsApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddAutoFluentValidation()
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.AllowTrailingCommas = true;
        option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

app.UseMiddleware<CustomExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();

