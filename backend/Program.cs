using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Services;
using backend.Controllers;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load the secret key from appsettings.json (better security)
var key = builder.Configuration["JwtSettings:Secret"] ?? "YourSuperSecureLongKey12345678901234";

// ✅ Add Database Context
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Enable CORS to allow React frontend requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// ✅ Register authentication with JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-app",
            ValidAudience = "your-audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// ✅ Register services
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

// ✅ Add controllers
//builder.Services.AddControllers();
builder.Services.AddControllers().AddApplicationPart(typeof(ScheduleController).Assembly);


var app = builder.Build();

// ✅ Apply CORS policy
app.UseCors("AllowFrontend");

// ✅ Apply authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// ✅ Map API controllers
app.MapControllers();

// ✅ Run the app
app.Run();
