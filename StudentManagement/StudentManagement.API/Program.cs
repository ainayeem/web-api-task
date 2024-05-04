
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentManagement.API.BGServices;
using StudentManagement.Persistence.Data;
using StudentManagement.Persistence.Interfaces;
using StudentManagement.Persistence.Repositories;
using System.Text;
using System.Xml.Linq;

namespace StudentManagement.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //logger
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        // worker
        builder.Services.AddHostedService<EmailService>();


        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddResponseCaching();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Management API", Version = "v1" });
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = "Oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
        //----------



        if (builder.Environment.IsDevelopment())
        {
            var dbName = "StudentsDb";
            builder.Services.AddDbContext<IApplicationDbContext, InMemoryDb>(
                options => options.UseInMemoryDatabase(dbName));

        }
        else
        {
            builder.Services.AddDbContext<StudentManagementDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("StudentManagementConnectionStrings")));

            builder.Services.AddScoped<IStudentRepository, SQLStudentRepository>();

            builder.Services.AddScoped<ITokenRepository, TokenRepository>();

        }

        // auth db di
        builder.Services.AddDbContext<StudentManagementAuthDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("StudentManagementAuthConnectionStrings")));

        
        //builder.Services.AddScoped<IStudentRepository, SQLStudentRepository>();

        //builder.Services.AddScoped<ITokenRepository, TokenRepository>();



        // identity soln.
        builder.Services.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("StudentManagement")
            .AddEntityFrameworkStores<StudentManagementAuthDbContext>()
            .AddDefaultTokenProviders();

        // identity options
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });



        // authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            });


        // cors
        builder.Services.AddCors(options => options.AddPolicy("TestCors", policy =>
        {
            //allow all orgins
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        //
        app.UseAuthentication();

        //
        app.UseCors("TestCors");

        //
        app.UseResponseCaching();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
