using Microsoft.EntityFrameworkCore;
using videoPortalAPI.Context;
using videoPortalAPI.Interfaces;
using videoPortalAPI.Repositories;
using videoPortalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
#region  DB injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion


#region Repositories
builder.Services.AddTransient<IVideoRepository, VideoRepository>();
#endregion

#region Services
builder.Services.AddTransient<IVideoService, VideoService>();
#endregion


#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowAll");
app.Run();
