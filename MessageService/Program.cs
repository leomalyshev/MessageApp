
using MessageService.Abstraction;
using MessageService.Context;
using MessageService.Mapping;
using MessageService.Repository;
using MessageService.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace MessageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                options =>
                {
                    options.AddSecurityDefinition(
                        JwtBearerDefaults.AuthenticationScheme,
                        new()
                        {
                            In = ParameterLocation.Header,
                            Description = "Please insert jwt with Bearer into filed",
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "Jwt Token",
                            Scheme = JwtBearerDefaults.AuthenticationScheme
                        });
                    options.AddSecurityRequirement(
                        new()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = JwtBearerDefaults.AuthenticationScheme
                                    }
                                },
                                new List<string>()
                            }

                        });
                });

            builder.Services.AddDbContext<MessageContext>(option =>
                option.UseNpgsql(builder.Configuration.GetConnectionString("db")));
            IConfiguration configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();

            var jwt = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()
                      ?? throw new Exception("JwtConfiguration not found");
            builder.Services.AddSingleton(provider => jwt);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    //IssuerSigningKey = jwt.GetSigningKey()

                    IssuerSigningKey = new RsaSecurityKey(RSATools.GetPublicKey(configuration))
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
