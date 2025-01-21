using DAL;
using DTOs;
using Exam.BackgroundServices;
using Exam.FileManager;
using Exam.Interfaces;
using Exam.ResultServices;
using MediatorServices.Abstract;
using MediatRepos;
using Microsoft.EntityFrameworkCore;
using Models;

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

            builder.Logging.AddProvider(new Log4NetProvider("log4net.config"));

            builder.Services.AddControllers().AddNewtonsoftJson();

            builder.Services.AddDbContext<Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient, ServiceLifetime.Transient);

            builder.Services.AddTransient<IRepository, ContextRepository>();
            builder.Services.AddTransient<IFileManager, FilesManager>();

            builder.Services.AddSingleton<IResultService, ResultService>();
            builder.Services.AddSingleton<IRequestStatusService, RequestStatusService>();
            builder.Services.AddSingleton<IGetService, GetService>();
            builder.Services.AddSingleton<IAddService, AddService>();
            builder.Services.AddSingleton<IUpdateService, UpdateService>();
            builder.Services.AddSingleton<IDownloadService, DownloadService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });


            builder.Services.AddSwaggerGen();

            builder.Services.AddHostedService(sp => sp.GetService<IGetService>() as GetService);
            builder.Services.AddHostedService(sp => sp.GetService<IAddService>() as AddService);
            builder.Services.AddHostedService(sp => sp.GetService<IUpdateService>() as UpdateService);
            builder.Services.AddHostedService(sp => sp.GetService<IDownloadService>() as DownloadService);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Add<VehicleDto>).Assembly);
            });

            var app = builder.Build();
            
            UpdateDatabase(app);

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
