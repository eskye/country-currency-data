using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonDataFilter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace JsonDataFilter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JsonDataController: ControllerBase
    {
        private readonly IHostEnvironment _environment;
        public JsonDataController(IHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> GetDataFiltered()

        {
            var countries = _environment.ContentRootPath + "/Dataset/countries.json";
            var country_currency = _environment.ContentRootPath + "/Dataset/country-currency.json";
            var currencies = _environment.ContentRootPath + "/Dataset/currencies.json";

            var countriesData = await System.IO.File.ReadAllTextAsync(countries);
            var country_currenciesData = await System.IO.File.ReadAllTextAsync(country_currency);
            var allCurrenciesData = await System.IO.File.ReadAllTextAsync(currencies);

            var countriesList = JsonConvert.DeserializeObject<List<CountryData>>(countriesData);
            var countriesCurrenciesList = JsonConvert.DeserializeObject<List<Currency>>(country_currenciesData);
            var allCurrenciesList = JsonConvert.DeserializeObject<List<AllCurrency>>(allCurrenciesData);

            var countriesCurrencyCodes = new List<CountryCurrencyCode>();
            countriesList.ForEach(c =>
                {
                    var currency = countriesCurrenciesList.SingleOrDefault(x => string.Equals(x.CountryName, c.Country, StringComparison.CurrentCultureIgnoreCase));
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
                var currency = allCurrenciesList.SingleOrDefault(x => string.Equals(x.Code, c.CurrencyCode, StringComparison.CurrentCultureIgnoreCase));
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
}