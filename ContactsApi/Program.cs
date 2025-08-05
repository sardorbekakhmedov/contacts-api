using ContactsApi.Middlewares;
using System.Text.Json.Serialization;
using ContactsApi.Data.Contexts;
using ContactsApi.Extensions;
using FluentValidation;
using ContactsApi.Dtos;
using ContactsApi.Validators;
using ContactsApi.Services.Contacts;
using Microsoft.EntityFrameworkCore;using EFCore.NamingConventions;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContext<ContactsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention());

var app = builder.Build();

app.UseMiddleware<CustomExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();

