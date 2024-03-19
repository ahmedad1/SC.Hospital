using Azure.Core;
using Hospital;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoryPattern.Core.Interfaces;
using RepositoryPattern.Core.Models;
using RepositoryPattern.EfCore;
using RepositoryPattern.EfCore.MailService;
using RepositoryPattern.EfCore.MapToModel;
using RepositoryPattern.EfCore.OptionPattenModels;
using RepositoryPattern.EfCore.Repositories;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.EfCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddDbContext<AppDbContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("constr")).UseLazyLoadingProxies());
builder.Services.AddTransient(typeof(IUnitOfWork),typeof(UnitOfWork));
var jwtOptions=builder.Configuration.GetSection("Jwt").Get<TokenOptionsModel>();
builder.Services.AddSingleton(jwtOptions!);
builder.Services.Configure<MailOptionsModel>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddSingleton<MapToUser>();
builder.Services.AddTransient<IMailService,MailService>();  
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
    };
    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = e =>
        {
            if (e.Request.Cookies.TryGetValue("jwt", out string? token))
            {
                e.Token = token;
            }
            return Task.CompletedTask;

        }
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<RedirectionToFrontEndMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
