using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace JsonDataFilter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHostEnvironment _environment;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetDataFiltered()
        
        {
            var countries = _environment.ContentRootPath + "/Dataset/countries.json";
            var country_currency = _environment.ContentRootPath + "/Dataset/country-currency.json";
            var currencies = _environment.ContentRootPath + "/Dataset/currencies.json";
            
            var countriesData = await System.IO.File.ReadAllTextAsync(countries);
            var currenciesData = await System.IO.File.ReadAllTextAsync(country_currency);
            var allCurrenciesData = await System.IO.File.ReadAllTextAsync(currencies);

            var countriesList = JsonConvert.DeserializeObject<List<CountryData>>(countriesData);
            var currencyList = JsonConvert.DeserializeObject<List<Currency>>(currenciesData); 
            var allCurrencyList = JsonConvert.DeserializeObject<List<AllCurrency>>(allCurrenciesData); 

            var countriesCurrencyCodes = new List<CountryCurrencyCode>();
            countriesList.ForEach(c =>
                {
                   var currency = currencyList.FirstOrDefault(x => string.Equals(x.CountryName, c.Country, StringComparison.CurrentCultureIgnoreCase));
                   if (currency != null)
                   {
                       countriesCurrencyCodes.Add(new CountryCurrencyCode
                       {
                           Country = c.Country,
                           CountryCode = currency.CountryCode,
                           CurrencyCode = currency.CurrencyCode
                       });
                   }
                });

            var response = new List<FilteredCountryData>();
            long i = 1;
            countriesCurrencyCodes.ForEach(c =>
            {
                var currency = allCurrencyList.FirstOrDefault(x => string.Equals(x.Code, c.CurrencyCode, StringComparison.CurrentCultureIgnoreCase));
                if (currency != null)
                {
                    response.Add(new FilteredCountryData
                    {
                        Country = c.Country,
                        CountryCode = c.CountryCode,
                        CurrencyCode = currency.Code,
                        CurrencyName = currency.Name,
                        CurrencySymbol = currency.Symbol,
                        CurrencySymbolNative = currency.SymbolNative,
                        Id = i
                    });
                }
                i++;
            });

            return Ok(response);
        }

    }

    public class CountryData
    {
        public string Country { get; set; }
    }

    public class Currency
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; } 
    }

    public class CountryCurrencyCode
    {
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class FilteredCountryData
    {
        public long Id { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbolNative { get; set; }
    }

    public class AllCurrency
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol_native")]
        public string SymbolNative { get; set; }

        [JsonProperty("decimal_digits")]
        public int DecimalDigits { get; set; }

        [JsonProperty("rounding")]
        public decimal Rounding { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name_plural")]
        public string NamePlural { get; set; }
    }
}
