using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using StripePaymentGateway.Settings;

namespace StripePaymentGateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var stripeSection = Configuration.GetSection(nameof(StripeSettings));
            var stripeSettings = stripeSection.Get<StripeSettings>();
            StripeConfiguration.ApiKey = stripeSettings.SecretKey;

            services.Configure<StripeSettings>(options =>
            {
                options.PublishableKey = stripeSettings.PublishableKey;
                options.SecretKey = stripeSettings.SecretKey;
            });

            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc(routes =>
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Payment}/{action=Index}")
                );
        }
    }
}
