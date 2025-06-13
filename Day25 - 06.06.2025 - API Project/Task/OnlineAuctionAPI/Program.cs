using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Contexts;
using OnlineAuctionAPI.Exceptions;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Mapping;
using OnlineAuctionAPI.Models;
using OnlineAuctionAPI.Repositories;
using OnlineAuctionAPI.Service;
using OnlineAuctionAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionAPI.Hubs;
using Serilog;
using Serilog.Events;
using System.Threading.RateLimiting;
using System.Security.Claims;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();


// builder.Host.UseSerilog(logger);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddEndpointsApiExplorer();

#region Swagger
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    }); 
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
#endregion

builder.Services.AddDbContext<AuctionContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//     });
// });



builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddAutoMapper(typeof(UserProfile));

#region Controllers
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                });
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomValidationFilter>();
});
#endregion

#region  API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});
#endregion

#region AuthenticationFilter
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = System.Text.Json.JsonSerializer.Serialize(new
                            {
                                success = false,
                                message = "Unauthorized",
                                data = (object)null,
                                errors = (object)null
                            });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });
#endregion

#region Repositories
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAuctionItemRepository, AuctionRepository>();
builder.Services.AddTransient<IBidItemRepository, BidItemRepository>();
builder.Services.AddTransient<AuctionRepository>();

#endregion

#region Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPasswordService, PasswordService>();
builder.Services.AddTransient<IAuctionItemService, AuctionItemService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthService, OnlineAuctionAPI.Service.AuthenticationService>();
builder.Services.AddTransient<IBidItemService, BidItemService>();
#endregion

#region CORS
builder.Services.AddCors(options=>{
    options.AddDefaultPolicy(policy=>{
        policy.WithOrigins("http://127.0.0.1:5500", "http://127.0.0.1:5501")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
#endregion

builder.Services.AddSignalR();

#region Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var user = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
        return RateLimitPartition.GetTokenBucketLimiter(user, _ =>
        {
            return new TokenBucketRateLimiterOptions
            {
                TokenLimit = 10,
                TokensPerPeriod = 1000,
                ReplenishmentPeriod = TimeSpan.FromHours(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
            };
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
#endregion

builder.Services.AddHttpContextAccessor();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();
// app.UseHttpsRedirection();
app.UseRateLimiter();
app.MapControllers();
app.UseCors();
app.MapHub<AuctionHub>("/auctionHub");

app.UseMiddleware<ExceptionMiddleware>();


app.Run();


// Case Study 13: Online Auction API
// Roles: Seller, Bidder
// Features:
// Post items for auction
// Place live bids
// SignalR for bid updates in real-time"