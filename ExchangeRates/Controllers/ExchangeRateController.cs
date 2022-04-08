using ExchangeRates.Repository.Contractors;
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
        public IConfiguration Configuration { get; }

        public ExchangeRateController(IExchangeRateRepository repository, IConfiguration configuration)
        {
            Repository = repository;
            Configuration = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DateTime from, DateTime to)
        {
            return Ok(await Repository.GetAll(from, to));
        }
    }
}
