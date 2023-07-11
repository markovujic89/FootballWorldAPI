using Application;
using Application.Core;
using Application.DTOs.Club;
using Application.DTOs.Image;
using Application.DTOs.LoginAndRegistration;
using Application.DTOs.Player;
using Application.Validations.Club;
using Application.Validations.Images;
using Application.Validations.Player;
using Application.Validations.Register;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Football world API", Version ="v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme =  "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<AuthDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnectionString"));
});

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddScoped<IPlayersService, PlayersService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IValidator<CreatePlayerDTO>, CreatePlayerValidator>();
builder.Services.AddScoped<IValidator<EditPlayerDTO>, EditPlayerValidation>();
builder.Services.AddScoped<IValidator<CreateClubDTO>, CreateClubValidator>();
builder.Services.AddScoped<IValidator<EditClubDTO>, EditClubValidator>();
builder.Services.AddScoped<IValidator<RequestRegisterDTO>, RequestRegisterValidator>();
builder.Services.AddScoped<IValidator<LoginRequestDTO>, RequestLoginValidator>();
builder.Services.AddScoped<IValidator<ImageUploadRequestDTO>, ImageUploadRequestValidator>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IImagesService, ImagesService>();

// RequestLoginValidator

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("FootballWorld")
    .AddEntityFrameworkStores<AuthDataContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// midlewere for enablin serving static file, in this case images
// use file provider, source where static file is served
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
