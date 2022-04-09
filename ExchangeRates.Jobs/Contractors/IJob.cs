using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Jobs.Contractors
{
    public interface IJob
    {
        public Task Start(); 
        public Task Stop();
    }
}
