using Azure.Core;
using Hospital;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RepositoryPattern.EfCore;
using RepositoryPattern.EfCore.MailService;
using RepositoryPattern.EfCore.MapToModel;
using RepositoryPattern.EfCore.OptionPattenModels;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.EfCore;
using RepositoryPatternWithUOW.EfCore.MapToModel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(x=>x.SerializerSettings.NullValueHandling=NullValueHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddDbContext<AppDbContext>(x=>x.UseSqlServer(builder.Configuration.GetConnectionString("constr")).UseLazyLoadingProxies());
builder.Services.AddTransient(typeof(IUnitOfWork),typeof(UnitOfWork));
var jwtOptions=builder.Configuration.GetSection("Jwt").Get<TokenOptionsModel>();
builder.Services.AddSingleton(jwtOptions!);
builder.Services.Configure<MailOptionsModel>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<ScheduleMapper>();
builder.Services.AddScoped<MapToUser>();
builder.Services.AddCors();
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["image/jpeg", "image/png", "image/x-png"]);
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();

app.UseHttpsRedirection();
app.UseCors(x => x.WithOrigins("http://localhost:3000").AllowCredentials().AllowAnyHeader().AllowAnyMethod());
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
