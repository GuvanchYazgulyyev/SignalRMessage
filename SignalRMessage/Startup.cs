using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRMessage.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMessage
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
            // Clint Çalışması için Politikalara izin vermemiz Gerekiyor.
            services.AddCors(options => options.AddDefaultPolicy(policy => policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials().SetIsOriginAllowed(origin => true)));

           // services.AddTransient<ChatHub>();

            services.AddSignalR();
            //services.AddControllers();
            //services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //https://localhost:5001
                endpoints.MapHub<ChatHub>("/chathub");
                //endpoints.MapControllers();
            });
        }
    }
}
