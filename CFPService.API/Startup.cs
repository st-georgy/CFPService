using CFPService.Application.Services;
using CFPService.Domain.Abstractions.Data;
using CFPService.Domain.Abstractions.Repositories;
using CFPService.Domain.Abstractions.Services;
using CFPService.Infrastructure.Data;
using CFPService.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CFPService.API
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddDbContext<ServiceDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString(nameof(ServiceDbContext))));

            services.AddScoped<IApplicationsRepository, ApplicationsRepository>();
            services.AddScoped<IDraftsRepository, DraftsRepository>();
            services.AddScoped<IApplicationsService, ApplicationsService>();
            services.AddScoped<IServiceDbContext, ServiceDbContext>();
            services.AddScoped<IActivitiesService, ActivitiesService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ServiceDbContext>();
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}
