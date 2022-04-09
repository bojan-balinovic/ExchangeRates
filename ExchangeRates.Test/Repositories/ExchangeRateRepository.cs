using ExchangeRates.Repository;
using ExchangeRates.Repository.Contractors;
using ExchangeRates.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRates.Test.Repositories
{
    public class ExchangeRateRepository
    {
        public IExchangeRateRepository Repository { get; }
        public MainContext Context { get; }

        public ExchangeRateRepository(IExchangeRateRepository exchangeRateRepository, MainContext context)
        {
            Repository = exchangeRateRepository;
            Context = context;
        }

        [Fact]
        public async void AddOne()
        {
            var exchangeRate = new ExchangeRate();
            exchangeRate.Base = "USD";
            exchangeRate.Code = "EUR";
            exchangeRate.Value = 1;
            exchangeRate.Date = DateTime.Today;
            var newExchangeRate=await Repository.AddOne(exchangeRate);
            Assert.NotNull(newExchangeRate);
        } 
        [Fact]
        public async void GetByDate_ShouldBeNull()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            var exchangeRate = await Repository.GetByDate(DateTime.Today);
            Assert.Null(exchangeRate);
        }
    }
}
