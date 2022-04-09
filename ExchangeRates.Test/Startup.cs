using ExchangeRates.Jobs;
using ExchangeRates.Jobs.Contractors;
using ExchangeRates.Repository;
using ExchangeRates.Repository.Contractors;
using ExchangeRates.Test.Jobs;
using Hangfire;
using Hangfire.SQLite.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExchangeRates.Test
{
    public class Startup
    {
        public Startup()
        {

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<MainContext>(options =>
            {
                options.UseInMemoryDatabase("test");
            });
            services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddTransient<IConsumeExchangeRateApiJob, MockUp>();
            services.AddSingleton<ITestOutputHelper, TestOutputHelper>();
  
        }
    }
}