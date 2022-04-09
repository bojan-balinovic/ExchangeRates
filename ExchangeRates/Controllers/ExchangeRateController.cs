using ExchangeRates.Repository.Contractors;
using ExchangeRates.Repository.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRates.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : ControllerBase
    {
        public IExchangeRateRepository Repository { get; }

        public ExchangeRateController(IExchangeRateRepository repository)
        {
            Repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime from, DateTime to)
        {
            return Ok(await Repository.GetAll(from, to));
        }

    }
}
