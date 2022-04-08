using ExchangeRates.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Repository
{
    public class MainContext:DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public MainContext(DbContextOptions options):base(options)
        {

        }
    }
}
