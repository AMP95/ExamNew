using DAL;
using DTOs;
using Exam.BackgroundServices;
using Exam.ResultServices;
using MediatRepos;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Exam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.AddProvider(new Log4NetProvider("log4net.config"));

            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient, ServiceLifetime.Transient);

            builder.Services.AddTransient<IRepository, ContextRepository>();

            builder.Services.AddSingleton<ResultService>();
            builder.Services.AddSingleton<RequestStatusService>();
            builder.Services.AddSingleton<GetService>();
            builder.Services.AddSingleton<UpdateService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });


            builder.Services.AddSwaggerGen();

            builder.Services.AddHostedService(sp => sp.GetService<GetService>());
            builder.Services.AddHostedService(sp => sp.GetService<UpdateService>());

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Add<TruckDto>).Assembly);
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("default");

            app.MapControllers();

            app.Run();
        }
    }
}
