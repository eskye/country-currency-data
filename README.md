# Country With Currency Mapped Dataset

This project combine different unstructured data of countries and their currencies to provide a structured data representation of countries with currency mapped together.

# What are we doing here?

The aim of this project is to use some existing unstructured dataset of countries and their currencies to generate a structured dataset that anyone looking for mapped country and currency together can use for any purpose.
 
 The final output of the dataset will contain:
 `CountryName, CurrencyName, CountryCode, CurrencyCode, CurrencySymbol, currencySymbolNative, iso` 

[Download Output dataset here](https://github.com/eskye/country-currency-data/blob/master/src/JsonDataFilter/output-data.json)


List of countries with currencies properties.

JSON object sample below:
```JSON
{
"id": 1,
"country": "Afghanistan",
"countryCode": "AF",
"currencyCode": "AFN",
"currencySymbol": "Af",
"currencyName": "Afghan Afghani",
"currencySymbolNative": "؋"
}

```

List of countries with currencies and ISO number.

JSON object sample below:
```JSON
{
"id": 1,
"country": "Afghanistan",
"countryCode": "AF",
"currencyCode": "AFN",
"currencySymbol": "Af",
"currencyName": "Afghan Afghani",
"currencySymbolNative": "؋",
"iso":4
}

```
# Usage & Download Dataset

Download the `country-currency-data.json` file and copy it to your project.

[Download List of countries with currencies properties](src/JsonDataFilter/Outputs/country-currency-data.json)
[Download List of countries with currencies and ISO number](src/JsonDataFilter/Outputs/country-currency-iso-data.json)

#### PHP

```PHP
$file = file_get_contents("./src/JsonDataFilter/Outputs/country-currency-data.json");

foreach (json_decode($file, true) as $key => $value) {
  var_dump($value); // {"id": 1,"country": "Afghanistan","countryCode": "AF","currencyCode": "AFN","currencySymbol": "Af","currencyName": "Afghan Afghani","currencySymbolNative": "؋"}
}
```

#### C#

```C#

   List<T> output;
    var stream = System.IO.File.Open("./src/JsonDataFilter/Outputs/country-currency-data.json", FileMode.Open);
    using (var streamReader = new StreamReader(stream))
    {
        var json = streamReader.ReadToEnd();
        var result = JsonConvert.DeserializeObject<List<T>>(json); // Where T is a class
        output = result.ToList();
     }

```

#### Node.js

```Javascript
var result = require('./src/JsonDataFilter/Outputs/country-currency-data.json')
console.log(result[0]); // {"id": 1,"country": "Afghanistan","countryCode": "AF","currencyCode": "AFN","currencySymbol": "Af","currencyName": "Afghan Afghani","currencySymbolNative": "؋"}

```

#### Python

```Python
import yaml

with open('./src/JsonDataFilter/Outputs/country-currency-data.json') as json_file:
    for line in yaml.safe_load(json_file):
        print line # // {"id": 1,"country": "Afghanistan","countryCode": "AF","currencyCode": "AFN","currencySymbol": "Af","currencyName": "Afghan Afghani","currencySymbolNative": "؋"}
```

# Contribute

Please feel free to send a pull request.

# Request

If any request, please raise an issue and it will be attended to in a very short period of time.

