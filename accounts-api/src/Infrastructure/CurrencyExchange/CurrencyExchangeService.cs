namespace Infrastructure.CurrencyExchange;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Services;
using Domain.ValueObjects;
using Newtonsoft.Json.Linq;

/// <summary>
///     Real implementation of the Exchange Service using external data source
/// </summary>
public sealed class CurrencyExchangeService : ICurrencyExchange
{
    public const string HttpClientName = "Fixer";

    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>")]
    private const string _exchangeUrl = "https://api.exchangeratesapi.io/latest?base=USD";

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly Dictionary<Currency, decimal> _usdRates = new Dictionary<Currency, decimal>();

    public CurrencyExchangeService(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    /// <summary>
    ///     Converts allowed currencies into USD.
    /// </summary>
    /// <returns>Money.</returns>
    public async Task<Money> Convert(Money originalAmount, Currency destinationCurrency)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientName);
        var requestUri = new Uri(_exchangeUrl);

        HttpResponseMessage response = await httpClient.GetAsync(requestUri)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        string responseJson = await response
            .Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        ParseCurrencies(responseJson);

        decimal usdAmount = _usdRates[originalAmount.Currency] / originalAmount.Amount;
        decimal destinationAmount = _usdRates[destinationCurrency] / usdAmount;

        return new Money(
            destinationAmount,
            destinationCurrency);
    }

    private void ParseCurrencies(string responseJson)
    {
        JObject rates = JObject.Parse(responseJson);
        var eur = rates["rates"]![Currency.Euro.Code]!.Value<decimal>();
        var cad = rates["rates"]![Currency.Canadian.Code]!.Value<decimal>();
        var gbh = rates["rates"]![Currency.BritishPound.Code]!.Value<decimal>();
        var sek = rates["rates"]![Currency.Krona.Code]!.Value<decimal>();
        var brl = rates["rates"]![Currency.Real.Code]!.Value<decimal>();

        _usdRates.Add(Currency.Dollar, 1);
        _usdRates.Add(Currency.Euro, eur);
        _usdRates.Add(Currency.Canadian, cad);
        _usdRates.Add(Currency.BritishPound, gbh);
        _usdRates.Add(Currency.Krona, sek);
        _usdRates.Add(Currency.Real, brl);
    }
}
