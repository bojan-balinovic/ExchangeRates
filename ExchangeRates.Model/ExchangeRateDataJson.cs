using System;
using System.Collections.Generic;

namespace ExchangeRates.Model
{
    public class ExchangeRateDataJson
    {
        public string Disclaimer { get; set; }
        public string License { get; set; }
        public long Timestamp { get; set; }
        public Dictionary<string, float> Rates { get; set; }
    }
}
