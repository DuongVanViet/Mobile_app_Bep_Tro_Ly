using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BepTroLy.Infrastructure.Persistence;
using BepTroLy.Infrastructure.Repositories;
using BepTroLy.Infrastructure.Security;
using BepTroLy.Application.Interfaces;
using BepTroLy.Application.Services;
using BepTroLy.API.Middlewares;
using BepTroLy.API.BackgroundServices;
using BepTroLy.Infrastructure.AI;
using BepTroLy.Infrastructure.Notification;

using BepTroLy.Infrastructure.Seeders;
using BepTroLy.API.Hubs;
using BepTroLy.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddScoped<BepTroLy.API.Services.IImageUploadService, BepTroLy.API.Services.ImageUploadService>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=BepTroLyDb;Trusted_Connection=True;"));

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings.GetValue<string>("Secret") ?? throw new InvalidOperationException("JWT Secret key not configured");
var issuer = jwtSettings.GetValue<string>("Issuer") ?? "BepTroLy";
var audience = jwtSettings.GetValue<string>("Audience") ?? "BepTroLyApp";
var expirationMinutes = jwtSettings.GetValue<int>("ExpirationMinutes");

builder.Services.AddSingleton(new JwtTokenProvider(secretKey, issuer, audience, expirationMinutes));

// JWT Authentication
var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Register infrastructure and application services
builder.Services.AddScoped<IngredientRepository>();
builder.Services.AddScoped<RecipeRepository>();
builder.Services.AddScoped<RecipeAiService>();
builder.Services.AddScoped<FirebasePushService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIngredientService, BepTroLy.Application.Services.IngredientService>();
builder.Services.AddScoped<IRecipeService, BepTroLy.Application.Services.RecipeService>();
builder.Services.AddScoped<IRecommendationService, BepTroLy.Application.Services.RecommendationService>();
builder.Services.AddScoped<IMealPlanService, MealPlanService>();
builder.Services.AddScoped<IShoppingListService, ShoppingListService>();
builder.Services.AddScoped<INotificationService, BepTroLy.Application.Services.NotificationService>();

// Background services
builder.Services.AddHostedService<ExpiryCheckService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

// Global exception middleware
app.UseMiddleware<ExceptionMiddleware>();

// Enable CORS
app.UseCors("AllowAll");

// JWT Authentication Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

// Initialize database with seed data
await DbInitializer.InitializeAsync(app.Services);

app.Run();

