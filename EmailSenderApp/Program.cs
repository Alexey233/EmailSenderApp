using EmailSenderApplication.Services.Implementations;
using EmailSenderApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using EmailSenderApplication.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmailService, EmailService>();



var connectionString = builder.Configuration.GetConnectionString("DbConnect");
builder.Services.AddDbContext<ApplicationContext>(c => c.UseNpgsql(connectionString));


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.MapControllers();

app.Run();
