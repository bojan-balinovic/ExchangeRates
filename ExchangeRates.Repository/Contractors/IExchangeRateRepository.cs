using ExchangeRates.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Repository.Contractors
{
    public interface IExchangeRateRepository
    {
        public Task<IEnumerable<ExchangeRate>> GetAll(DateTime from, DateTime to);
        public Task<ExchangeRate> AddOne(ExchangeRate exchangeRate);
        public Task<ExchangeRate> GetOne(int id);
        public Task<ExchangeRate> GetByDate(DateTime date);


    }
}
