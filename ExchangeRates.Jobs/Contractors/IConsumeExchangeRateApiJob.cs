using ExchangeRates.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Jobs.Contractors
{
    public interface IConsumeExchangeRateApiJob
    {
        public Task Consume(DateTime? date=default);
        Task<ExchangeRateDataJson> MakeGetRequest(DateTime exchangeRateDate);
    }
}
