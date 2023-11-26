using ChattingApplication.Core;
using ChattingApplication.Core.Repositories;
using ChattingApplication.Persistence;
using ChattingApplication.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ChattingApplication.DependencyInjection;
using ChattingApplication.Core.EmailService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace ChattingApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.Configure<MvcNewtonsoftJsonOptions>(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddRepositories(builder.Configuration);

            builder.Configuration.AddUserSecrets("8338e13c-00f0-4af7-b092-5cdb374b6648");

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddSingleton<IEmailService, EmailService>();

            builder.Services.AddSignalR();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
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

            app.MapHub<ChatHub>("/chatHub");

            app.MapControllers();

            app.Run();
        }
    }
}