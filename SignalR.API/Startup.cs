using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.API.Data;
using SignalR.API.Hubs;

namespace SignalR.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SignalRContext")));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                 {
                     builder.WithOrigins("https://localhost:44363").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                 });
            });

            services.AddControllers();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy"); //api'nin web client taraf�ndan gelen istekleri cevapl�yabilmesi i�in gerekli service ve middleware eklemeleri tamamland�.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HubClass>("/HubClass"); //clientler huba baglanmak i�in http://*:5000/HubClass diye baglanmas� gerekti�i i�in
            });
        }
    }
}
