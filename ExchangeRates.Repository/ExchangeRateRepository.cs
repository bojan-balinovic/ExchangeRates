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
            await Context.SaveChangesAsync();
            return exchangeRate;
        }

        public async Task<IEnumerable<ExchangeRate>> GetAll(DateTime from=default(DateTime), DateTime to=default(DateTime))
        {
            if (from == default(DateTime) && to == default(DateTime))
            {
                return Context.ExchangeRates.ToList();
            }
            if (from == default(DateTime))
            {
                from = DateTime.Today;
            }
            if(to == default(DateTime))
            {
                to = DateTime.Today;
            }
         
            var exchangeRates = Context.ExchangeRates.Where(e => e.Date >= from.Date && e.Date <= to.Date);
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
            return exchangeRate;
        }
    }
}
