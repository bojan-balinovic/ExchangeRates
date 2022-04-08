using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Repository.Entities
{
    public class ExchangeRate
    {
        [Key]
        public int Id { get; set; }
        public string Base { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public float Value { get; set; }
    }
}
