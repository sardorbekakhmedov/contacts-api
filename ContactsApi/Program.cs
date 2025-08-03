using ContactsApi.Middlewares;
using System.Text.Json.Serialization;
using ContactsApi.Extensions;
using FluentValidation;
using ContactsApi.Dtos;
using ContactsApi.Validators;
using ContactsApi.Services.Contacts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddAutoFluentValidation()
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.AllowTrailingCommas = true;
        option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSingleton<IContactService, ContactService>();
builder.Services.AddScoped<IValidator<CreateContactDto>, CreateContactValidator>();
builder.Services.AddScoped<IValidator<UpdateContactDto>, UpdateContactValidator>();
builder.Services.AddScoped<IValidator<PatchContactDto>, PatchContactValidator>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CustomExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();

