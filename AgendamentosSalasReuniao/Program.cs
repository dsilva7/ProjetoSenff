using Aplicacao.Services;
using Aplicacao.Validators;
using Common;
using FluentValidation;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using SendGrid;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SenffContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure())
);


// Configuração do SendGrid a partir do appsettings.json
//.Services.AddSingleton<ISendGridClient>(new SendGridClient(builder.Configuration["SendGrid:ApiKey"]));

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<SalasService>();
builder.Services.AddScoped<ReservasService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddValidatorsFromAssemblyContaining<CadastroSalaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EfetivarReservaValidator>();

builder.Services.AddControllers();
// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
