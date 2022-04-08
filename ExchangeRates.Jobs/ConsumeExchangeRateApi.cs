using ExchangeRates.Jobs.Contractors;
using ExchangeRates.Model;
using ExchangeRates.Repository;
using ExchangeRates.Repository.Contractors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates.Jobs
{
    public class ConsumeExchangeRateApiJob : IConsumeExchangeRateApiJob
    {
        public IExchangeRateRepository Repository { get; }
        public IConfiguration Configuration { get; }

        public ConsumeExchangeRateApiJob( IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                Repository = new ExchangeRateRepository(scope.ServiceProvider.GetRequiredService<MainContext>());
                Configuration = configuration;
            }
        }

        public virtual async Task Consume(DateTime? date=default)
        {
            if (date == default || date==null) date = DateTime.Today;
            if ((DateTime.Today - date.Value).TotalDays >= 6) return;
            Debug.Write(date.Value.ToUniversalTime());

            if (Repository.GetByDate(date.Value)!=null) {
                ExchangeRateDataJson data = await MakeGetRequest(date.Value);
                Debug.WriteLine(JsonConvert.SerializeObject(data).ToString());
            }

            await Consume(date.Value.AddDays(-1));
        }

        public virtual async Task<ExchangeRateDataJson> MakeGetRequest(DateTime exchangeRateDate)
        {
            using (var httpClient = new HttpClient())
            {
                var apiKey = Configuration["OpenexchangeRates:ServiceApiKey"];

                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(
                    string.Format("https://openexchangerates.org/api/historical/{0}.json?app_id={1}", exchangeRateDate.ToString("yyyy-MM-dd"), apiKey)
                    );
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                string jsonString = await (await httpClient.SendAsync(request)).Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ExchangeRateDataJson>(jsonString);
                return data;
            }
        }
    }
}
