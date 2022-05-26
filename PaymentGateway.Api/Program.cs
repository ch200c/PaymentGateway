using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Api.Services;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Interfaces.Repositories;
using PaymentGateway.Application.Validators;
using PaymentGateway.Domain.Utils;
using PaymentGateway.Infrastructure.Persistence;
using PaymentGateway.Infrastructure.Persistence.Repositories;
using PaymentGateway.Infrastructure.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddFluentValidation(configuration =>
        configuration.RegisterValidatorsFromAssemblyContaining<ProcessPaymentRequestValidator>())
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning();
builder.Services.AddVersionedApiExplorer(options =>
{
    options.SubstituteApiVersionInUrl = true;
    options.GroupNameFormat = "'v'VVV";
});

builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(builder =>
    builder.UseInMemoryDatabase("PaymentGateway"));

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IPaymentDetailsRepository, PaymentDetailsRepository>();

builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddSingleton<IAcquiringBankClient, AcquiringBankClient>();

builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddTransient(_ => new CardNumberPrivacyFilter('X'));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.MapControllers();

app.Run();
