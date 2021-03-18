using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Timesheet.Api.Services;
using Timesheet.Application.Services;
using Timesheet.DataAccess;
using Timesheet.DataAccess.Repositories;
using Timesheet.Domain.Abstractions;

namespace Timesheet.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        private readonly IConfiguration _configuration;
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                    options.UseNpgsql(_configuration["Data:ConnectionString"]);
            });
            
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ITimeLogRepository, TimeLogRepository>();
            
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IReportService, ReportService>();
            
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timesheet API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Timesheet API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}