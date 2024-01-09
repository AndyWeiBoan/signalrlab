using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SignalrLab.Hubs;
using SignalrLab.Services;

namespace SignalrLab {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddSignalR();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //{
            //    options.SecurityTokenValidators.Clear();
            //    options.SecurityTokenValidators.Add(new CustomSecurityTokenValidator());
            //}); ;
            //services.AddHostedService<TestHostedService>();
            //services.AddSingleton<AuthenticationService, CustomAuthService>();
            services.AddSingleton<IUserIdProvider, PlayerIdProvider>();
            services.AddSingleton<IHubCallerManager, HubCallerManager>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.AddScheme<MyAuthenticationHandler>(MyAuthenticationHandler.SchemeName, "chatting");
            });

            //services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, MyAuthenticationHandler>(MyAuthenticationHandler.SchemeName, options => {
            //    //这里配置你的AuthenticationHandler
            //    //options.
            //    options.
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                //app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                //endpoints.MapGet("/", async context => {
                //    await context.Response.WriteAsync("Hello World!");
                //});
                endpoints.MapHub<Hubs.ChatHub>("/chatHub");
            });
        }
    }
}
