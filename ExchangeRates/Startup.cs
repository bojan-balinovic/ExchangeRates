using ExchangeRates.Jobs;
using ExchangeRates.Jobs.Contractors;
using ExchangeRates.Repository;
using ExchangeRates.Repository.Contractors;
using Hangfire;
using Hangfire.SQLite.Core;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExchangeRates
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExchangeRates", Version = "v1" });
            });
            services.AddDbContext<MainContext>(options =>
            {
                options.UseSqlite("Data source = database.db");
            });
            services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddTransient<IConsumeExchangeRateApiJob, ConsumeExchangeRateApiJob>();
            services.AddHangfire(options =>
            {
                options.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseColouredConsoleLogProvider()
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                  .UseSQLiteStorage();

            });
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExchangeRates v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();

            var reccuringJobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();
            var consumeExchangeRateApiJob = app.ApplicationServices.GetRequiredService<IConsumeExchangeRateApiJob>();
            reccuringJobManager.AddOrUpdate(
                "get-exchange-rates-from-3rd-party-api",
                () => consumeExchangeRateApiJob.Consume(DateTime.Today),
               "0 * * ? * *");
        }
        public static void DoSomething()
        {
            Console.WriteLine("something");
            Debug.Write("something");
        }

    }
}
