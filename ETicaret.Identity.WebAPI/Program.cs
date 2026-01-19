using ETicaret.Gateway.YARP.Context;
using ETicaret.Gateway.YARP.Dtos;
using ETicaret.Gateway.YARP.Models;
using ETicaret.Identity.Context;
using ETicaret.Identity.Dtos;
using ETicaret.Identity.Models;
using ETicaret.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
});

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("JWT:SecretKey").Value ?? ""
            )
        ),
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtProvider>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.MapGet("/", () => "Identity Service is running");

// REGISTER
app.MapPost("/register", async (
    RegisterDto request,
    ApplicationDbContext context,
    CancellationToken cancellationToken) =>
{
    bool isUserNameExists = await context.Users
        .AnyAsync(p => p.UserName == request.UserName, cancellationToken);

    if (isUserNameExists)
    {
        return Results.BadRequest(Result<string>.Failure("Kullanýcý adý daha önce alýnmýþ"));
    }

    User user = new()
    {
        UserName = request.UserName,
        Password = request.Password 
    };

    await context.AddAsync(user, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(Result<string>.Succeed("Kullanýcý kaydý baþarýlý"));
});

// LOGIN
app.MapPost("/login", async (
    LoginDto request,
    ApplicationDbContext context,
    JwtProvider jwtProvider,
    CancellationToken cancellationToken) =>
{
    User? user = await context.Users
        .FirstOrDefaultAsync(p => p.UserName == request.UserName, cancellationToken);

    if (user is null)
    {
        return Results.BadRequest(Result<string>.Failure("Kullanýcý bulunamadý"));
    }

    string token = jwtProvider.CreateToken(user);
    return Results.Ok(Result<string>.Succeed(token));
});

app.UseAuthentication();
app.UseAuthorization();

// Migration
using (var scope = app.Services.CreateScope())
{
    var srv = scope.ServiceProvider;
    var context = srv.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
