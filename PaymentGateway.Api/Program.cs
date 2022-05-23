using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Interfaces.Repositories;
using PaymentGateway.Application.Validators;
using PaymentGateway.Infrastructure;
using PaymentGateway.Infrastructure.Repositories;
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

builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(builder => 
    builder.UseInMemoryDatabase("PaymentGateway"));

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IPaymentDetailsRepository, PaymentDetailsRepository>();

builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddSingleton<IAcquiringBankClient, AcquiringBankClient>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
