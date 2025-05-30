using BankingApp.Contexts;
using BankingApp.Interfaces;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;
using BankingApp.Repositories;
using BankingApp.Services;
using System.Text.Json.Serialization;
using BankingApp.Misc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
               
  

builder.Services.AddDbContext<BankingContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IRepository<int, Customer>, CustomerRepository>();
builder.Services.AddTransient<IRepository<int, Account>, AccountRepository>();
builder.Services.AddTransient<IRepository<int, Transaction>, TransactionRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddScoped<IRepository<int, Account>, AccountRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddScoped<IOtherFunctionalitiesInterface, OtherFunctionalities>();
builder.Services.AddSingleton<ChatbotService>();
builder.Services.AddScoped<ModelTrainer>();







var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        Console.WriteLine("Starting model training...");
        var trainer = scope.ServiceProvider.GetRequiredService<ModelTrainer>();
        trainer.TrainAndSaveModel();
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Model training failed: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();



app.Run();


