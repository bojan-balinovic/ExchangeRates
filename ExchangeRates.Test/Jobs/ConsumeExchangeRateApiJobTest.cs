using ExchangeRates.Jobs;
using ExchangeRates.Jobs.Contractors;
using ExchangeRates.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ExchangeRates.Test.Jobs
{
    public class MockUp : ConsumeExchangeRateApiJob, IConsumeExchangeRateApiJob
    {
        public MockUp(IConfiguration configuration, IServiceScopeFactory scopeFactory):base(configuration, scopeFactory)
        {

        }

        public ITestOutputHelper TestOutputHelper { get; set; }

        public override Task Consume(DateTime? date = null)
        {
            TestOutputHelper.WriteLine(date.Value.ToUniversalTime().ToString());   
            return base.Consume(date);
        }
        public override Task<ExchangeRateDataJson> MakeGetRequest(DateTime exchangeRateDate)
        {
            return null;
        }
    }
    public class ConsumeExchangeRateApiJobTest
    {
        public ConsumeExchangeRateApiJobTest(IConsumeExchangeRateApiJob mockUp, ITestOutputHelper testOutputHelper)
        {
            MockUp = mockUp;
            ((MockUp)MockUp).TestOutputHelper = testOutputHelper;
            TestOutputHelper = testOutputHelper;
        }

        public IConsumeExchangeRateApiJob MockUp { get; }
        public ITestOutputHelper TestOutputHelper { get; }

        [Fact]
        public void DoesRecursionWork()
        {
            MockUp.Consume(DateTime.Today);
            Assert.Null( MockUp.MakeGetRequest(default));
        }
    }
}
