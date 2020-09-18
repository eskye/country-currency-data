namespace JsonDataFilter.Models
{
    public class FilteredCountryData
    {
        public long Id { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbolNative { get; set; }
        public int? Iso { get; set; }
    }
}
