using DAL;
using DTOs;
using DTOs.Dtos;
using Exam.Authentication;
using Exam.FileManager;
using Exam.ResultServices;
using Exam.Services;
using Exam.Services.BackgroundServices;
using Logger4Net;
using MediatR;
using MediatRepos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;
using Utilities.Interfaces;

namespace Exam
{
    public class Program
    {
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<Context>())
                {
                    context.Database.Migrate();
                }
            }
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            TokenSettings tokenSettings = new TokenSettings();
            builder.Configuration.GetSection("TokenSettings").Bind(tokenSettings);
            JwtTokenService.LoadSettings(tokenSettings);

            builder.Logging.AddProvider(new Log4NetProvider("logger.config"));

            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient, ServiceLifetime.Transient);

            builder.Services.AddTransient<IRepository, ContextRepository>();
            builder.Services.AddTransient<IFileManager, FilesManager>();
            builder.Services.AddTransient<IContractCreator<ContractDto, CompanyBaseDto>, SpireContractCreator>();
            builder.Services.AddTransient<ITokenService<LogistDto>, JwtTokenService>();
            builder.Services.AddTransient<IHashService, HashService>();
            builder.Services.AddTransient<IAppRootResolver, RootResolver>();

            builder.Services.AddSingleton<IQueueService<IRequest<IServiceResult<object>>>, QueueService>();
            builder.Services.AddSingleton<IResultService, ResultService>();
            builder.Services.AddSingleton<IStatusService, StatusService>();
            

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                           .AddJwtBearer(options =>
                           {
                               options.RequireHttpsMetadata = false;
                               options.TokenValidationParameters = new TokenValidationParameters
                               {
                                   ValidateIssuer = true,
                                   ValidIssuer = JwtTokenService.ISSUER,

                                   ValidateAudience = true,
                                   ValidAudience = JwtTokenService.AUDIENCE,
                                   ValidateLifetime = true,

                                   IssuerSigningKey = JwtTokenService.GetSymmetricSecurityKey(),
                                   ValidateIssuerSigningKey = true,
                               };
                           });


            builder.Services.AddSwaggerGen();

            builder.Services.AddHostedService(sp => sp.GetService<IQueueService<IRequest<IServiceResult<object>>>>() as QueueService);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Update<VehicleDto>).Assembly);
            });

            var app = builder.Build();
            
            UpdateDatabase(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("default");

            app.MapControllers();

            app.Run();
        }
    }
}
