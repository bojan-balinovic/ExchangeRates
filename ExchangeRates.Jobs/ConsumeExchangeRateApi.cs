using ExchangeRates.Jobs.Contractors;
using ExchangeRates.Model;
using ExchangeRates.Repository;
using ExchangeRates.Repository.Contractors;
using ExchangeRates.Repository.Entities;
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

        public IServiceScopeFactory ScopeFactory { get; }

        public ConsumeExchangeRateApiJob(IServiceScopeFactory scopeFactory)
        {
            ScopeFactory = scopeFactory;
        }

        public string GetApiUrl(DateTime date, string appId)
        {
            return string.Format("https://openexchangerates.org/api/historical/{0}.json?app_id={1}", date.ToString("yyyy-MM-dd"), appId);
        }

        public async Task Start()
        {
            await Consume();
        }

        public Task Stop()
        {
            return null;
        }

        public virtual async Task Consume(DateTime? date = default)
        {

            using (var scope = ScopeFactory.CreateScope())
            {
                var repository = new ExchangeRateRepository(scope.ServiceProvider.GetRequiredService<MainContext>());
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                if (date == default || date == null) date = DateTime.Today;
                if ((DateTime.Today - date.Value).TotalDays >= 7) return;

                if (await repository.GetByDate(date.Value) == null)
                {
                    ExchangeRateDataJson data;
                    try
                    {
                        data = await MakeGetRequest(date.Value);
                    }
                    catch (Exception ex)
                    {
                        return;
                    }

                    var euro = new ExchangeRate();
                    euro.Base = data.Base;
                    euro.Code = "EUR";
                    euro.Value = data.Rates["EUR"];
                    euro.Date = date.Value;
                    await repository.AddOne(euro);

                    var gbp = new ExchangeRate();
                    gbp.Code = "GBP";
                    gbp.Value = data.Rates["GBP"];
                    gbp.Date = date.Value;
                    await repository.AddOne(gbp);

                }
            }
            await Consume(date.Value.AddDays(-1)); //recursion
        }

        public virtual async Task<ExchangeRateDataJson> MakeGetRequest(DateTime exchangeRateDate)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                using (var httpClient = new HttpClient())
                {
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var apiKey = configuration["OpenexchangeRates:ServiceApiKey"];


                    var request = new HttpRequestMessage();
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri(
                       GetApiUrl(exchangeRateDate, apiKey)
                        );
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                    );
                    string jsonString = await (await httpClient.SendAsync(request)).Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ExchangeRateDataJson>(jsonString);
                    if (data.Error == true)
                    {
                        throw new Exception("No api key defined in user secrets.");
                    }
                    return data;
                }
            }
        }

    }
}
