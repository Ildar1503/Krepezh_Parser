using AngleSharp;
using AngleSharp.Dom;
using KrepParser.Application.DTO;
using KrepParser.Domain.Enums;
using KrepParser.Domain.Shared;
using KrepParser.Infrastruction.ParseConfiguration.Primitives;
using KrepParser.Infrastruction.Shared;

namespace KrepParser.Infrastruction.ParseConfiguration
{
    public sealed class LevshaParseConfiguration : BaseParseConfiguration, IParseConfiguration<ParseProductDTO>
    {
        private const string _ShopName = "Левша"; 

        private static string pageCountUrl = string.Empty; 

        public LevshaParseConfiguration
            (string url, string request)
        {
            URL = url;
            Request = request;

            pageCountUrl = $"{url}?q={request}&PAGEN_2=1";
        }

        public async Task<ParseResult<int>> GetPageCountAsync()
        {
            int pageCount = 0;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, pageCountUrl);
                var response = await client.SendAsync(request);

                if (response == null)
                    return ParseResult<int>.FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.ResponseIsNull));

                var content = await response.Content.ReadAsStreamAsync();

                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(req => req.Content(content));

                var result = document.QuerySelectorAll(".modern-page-navigation > a");

                if (result.Length == 0 || result == null)
                    return ParseResult<int>
                        .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.SelectionIsEmpty));

                pageCount = result.Length >= 1 ? Convert.ToInt32(result[result.Length - 2].TextContent) : 1;
            }

            return ParseResult<int>.Succes(pageCount);
        }

        public async Task<ParseResult<List<ParseProductDTO>>> GetParseResultAsync()
        {
            int pageCount = GetPageCountAsync().Result.Error == ParseError.Ok ?
                    GetPageCountAsync().Result.TValue : -1;

            //Проверка колличества страниц
            if (pageCount == -1)
                return ParseResult<List<ParseProductDTO>>
                    .FailureWithotValue(new ParseError(ParseErrors.ParseResultNullOrEmpty, ParseErrorsMessage.CannotFoundPageCount));

            using (var client = new HttpClient())
            {
                //Результат парсинга.
                var parseResult = new List<ParseProductDTO>();
                //Листы названий и цен.
                var names = new List<IHtmlCollection<IElement>>();
                var prices = new List<IHtmlCollection<IElement>>();
                //Лист для записи корректной цены.
                var correctPrices = new List<List<double>>();

                for (int i = 1; i <= pageCount; i++)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}?q={Request}&PAGEN_2={i}");
                    var response = await client.SendAsync(request);

                    if (response == null)
                        return ParseResult<List<ParseProductDTO>>
                            .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.ResponseIsNull));

                    var content = await response.Content.ReadAsStreamAsync();

                    var config = Configuration.Default.WithDefaultLoader();
                    var context = BrowsingContext.New(config);
                    var document = await context.OpenAsync(req => req.Content(content));

                    var name = document.QuerySelectorAll(".pi-title");
                    var price = document.QuerySelectorAll(".pi-price > li");

                    if ((name.Length == 0 || name == null) || (price.Length == 0 || price == null))
                        return ParseResult<List<ParseProductDTO>>
                            .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.SelectionIsEmpty));

                    names.Add(name);
                    prices.Add(price);
                }

                correctPrices = GetCorrectCost(prices);

                if (correctPrices == null)
                {
                    return ParseResult<List<ParseProductDTO>>
                        .FailureWithotValue(new ParseError(ParseErrors.Convert, ParseErrorsMessage.InvalidPriceConvert));
                }

                //Запись в итоговый лист
                for (int i = 0; i < names.Count; i++)
                {
                    for (int j = 0; j < names[i].Length; j++)
                    {
                        parseResult.Add(ParseProductDTO
                            .Create(names[i][j].TextContent, correctPrices[i][j], _ShopName));
                    }
                }

                return ParseResult<List<ParseProductDTO>>
                    .Succes(parseResult);
            }
        }

        //Приводит в порядок цену.
        private List<List<double>> GetCorrectCost(List<IHtmlCollection<IElement>> price)
        {
            var result = new List<List<double>>();

            for (int i = 0; i < price.Count; i++)
            {
                List<double> tempCost = new();

                for (int j = 0; j < price[i].Length; j += 2)
                {
                    var sRes = price[i][j].TextContent.Replace(" ", "");
                    string num = " ";

                    for (int s = 1; s < sRes.Length; s++)
                    {
                        if (!char.IsDigit(sRes[s]) && sRes[s] != '.') break;

                        if (sRes[s] == '.') num += ',';
                        else
                            num += sRes[s];
                    }

                    double currenPrice = num == " " ? -1 : Convert.ToDouble(num);
                    tempCost.Add(currenPrice);
                }

                result.Add(tempCost);
            }

            return result;
        }
    }
}
