using Homely.HackDays.ListingsAI.WebUI.Configuration;
using Homely.HackDays.ListingsAI.WebUI.Services.ContentModeration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;
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

            var contentModeratorClient = new ContentModeratorClient(new ApiKeyServiceClientCredentials(azureSettings.Get<AzureSettings>().ContentModeratorApiKey))
            {
                BaseUrl = "australiaEast.api.cognitive.microsoft.com"
            };

            services.AddSingleton<IContentModeratorClient>(contentModeratorClient);
            services.AddSingleton<IContentModerationService, ContentModerationService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
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
                    template: "{controller=ContentModerator}/{action=ValidateText}/{id?}");
            });
        }
    }
}