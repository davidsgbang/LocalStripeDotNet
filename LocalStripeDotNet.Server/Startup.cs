using System.Text.Json;
using System.Text.Json.Serialization;
using LocalStripeDotNet.Server.Facades;
using LocalStripeDotNet.Server.Generators;
using LocalStripeDotNet.Server.Repositories;
using LocalStripeDotNet.Server.Repositories.InMemory;
using LocalStripeDotNet.Server.Webhooks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Stripe.Infrastructure;
using IssuingCard = Stripe.Issuing.Card;
using IssuingCardholder = Stripe.Issuing.Cardholder;
using UnixDateTimeConverter = Stripe.Infrastructure.UnixDateTimeConverter;

namespace LocalStripeDotNet.Server
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
            services.AddSingleton<IStripeRepository<IssuingCardholder>, InMemoryIssuingCardholderRepository>();
            services.AddSingleton<IStripeRepository<IssuingCard>, InMemoryIssuingCardRepository>();

            var webhookTarget = this.Configuration.GetValue<string>("webhookTarget") ?? "https://localhost:5005";
            
            services.AddSingleton<IWebhookInitiator>(new WebhookInitiator(webhookTarget));

            services.AddSingleton<IssuingCardholderFacade>();
            services.AddSingleton<IssuingCardFacade>();
            
            services.AddControllers()
                .AddJsonOptions(options => { 
                    options.JsonSerializerOptions.PropertyNamingPolicy = 
                        JsonNamingPolicy.CamelCase;
                });
            services.AddMvc()
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                    opts.SerializerSettings.Converters.Add(new UnixDateTimeConverter());
                });
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LocalStripe.NET V1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LocalStripe.NET V1"); 
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}