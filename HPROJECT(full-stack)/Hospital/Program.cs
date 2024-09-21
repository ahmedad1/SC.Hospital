using Azure.Core;
using Hospital;
using Hospital.PaymobHmacService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RepositoryPattern.EfCore;
using RepositoryPattern.EfCore.MailService;
using RepositoryPattern.EfCore.MapToModel;
using RepositoryPattern.EfCore.OptionPattenModels;
using RepositoryPatternUOW.Core.OptionPattern;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.EfCore;
using RepositoryPatternWithUOW.EfCore.InitPayService;
using RepositoryPatternWithUOW.EfCore.MapToModel;
using System.Text;
using The_Wedding.PaymobHmacService;

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
builder.Services.AddHttpClient();
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
builder.Services.AddScoped<IInitPaymentService,InitPayService>();   
builder.Services.Configure<PaymobInitOptions>(builder.Configuration.GetSection("PaymobInit"));
builder.Services.Configure<PaymobHmac>(builder.Configuration.GetSection("PaymobHmac"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["image/jpeg", "image/png", "image/x-png"]);
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.AddScoped<IPaymobHmacService,PaymobHmacService>();
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
