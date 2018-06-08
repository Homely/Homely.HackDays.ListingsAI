using Homely.HackDays.ListingsAI.WebUI.Configuration;
using Homely.HackDays.ListingsAI.WebUI.Models;
using Homely.HackDays.ListingsAI.WebUI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Homely.HackDays.ListingsAI.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var azureSettings = Configuration.GetSection(AzureSettings.ConfigurationKey);
            services.Configure<AzureSettings>(azureSettings);

            services.AddHttpClient<HomelyListingsClient>();
            services.AddHttpClient<AzureCognitiveClient>(c =>
            {
                c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", 
                                            azureSettings.Get<AzureSettings>().TextAnalyticsApiKey);
            });

            services.AddSingleton<IKeyPhraseExtractionService>(s => 
                new KeyPhraseExtractionService("AppData/tags.txt",
                                               s.GetService<HomelyListingsClient>(),
                                               s.GetService<AzureCognitiveClient>()));
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
