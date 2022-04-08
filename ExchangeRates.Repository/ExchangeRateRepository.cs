using ExchangeRates.Repository.Contractors;
using ExchangeRates.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRates.Repository
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        public MainContext Context { get; }
        public ExchangeRateRepository(MainContext context)
        {
            Context = context;
        }

        public async Task<ExchangeRate> AddOne(ExchangeRate exchangeRate)
        {
            await Context.ExchangeRates.AddAsync(exchangeRate);
            return exchangeRate;
        }

        public async Task<IEnumerable<ExchangeRate>> GetAll(DateTime? from, DateTime? to)
        {
            if (from == null)
            {
                from = DateTime.Today;
            }
            if(to == null)
            {
                to = DateTime.Today;
            }
            var exchangeRates = Context.ExchangeRates.Where(e => e.Date >= from.Value.Date && e.Date <= to.Value.Date);
            return exchangeRates;
        }

        public async Task<ExchangeRate> GetOne(int id)
        {
            var exchangeRate = await Context.ExchangeRates.FindAsync(id);
            return exchangeRate;
        }
        public async Task<ExchangeRate> GetByDate(DateTime date)
        {
            var exchangeRate = Context.ExchangeRates.Where(e=>e.Date==date).FirstOrDefault();
            return await Task.FromResult(exchangeRate);
        }
    }
}
