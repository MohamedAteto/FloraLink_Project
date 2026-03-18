using System.Text;
using FloraLink.Application.Interfaces;
using FloraLink.Application.Services;
using FloraLink.Domain.Interfaces;
using FloraLink.Infrastructure.Data;
using FloraLink.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var usePostgres = Environment.GetEnvironmentVariable("USE_POSTGRES") == "true" ||
                  (connectionString != null && connectionString.StartsWith("Host="));

builder.Services.AddDbContext<FloraLinkDbContext>(options =>
{
    if (usePostgres)
        options.UseNpgsql(connectionString);
    else
        options.UseSqlServer(connectionString);
});

// ── Repositories ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<IPlantTypeRepository, PlantTypeRepository>();
builder.Services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
builder.Services.AddScoped<IWateringRepository, WateringRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IDiaryRepository, DiaryRepository>();

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPlantService, PlantService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<IWateringService, WateringService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IDiaryService, DiaryService>();
builder.Services.AddScoped<IPlantTypeService, PlantTypeService>();

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// ── CORS (allow React dev server) ─────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FloraLinkPolicy", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── Swagger ───────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FloraLink API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ── Migrate & seed on startup ─────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FloraLinkDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch
    {
        // Tables may already exist — ensure DB is created and continue
        db.Database.EnsureCreated();
    }
    DatabaseSeeder.Seed(db);
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("FloraLinkPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
