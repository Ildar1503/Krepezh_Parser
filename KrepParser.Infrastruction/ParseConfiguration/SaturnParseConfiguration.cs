using AngleSharp.Dom;
using AngleSharp;
using KrepParser.Application.DTO;
using KrepParser.Domain.Shared;
using KrepParser.Infrastruction.ParseConfiguration.Primitives;
using System;
using KrepParser.Domain.Enums;
using AngleSharp.Html.Dom;
using KrepParser.Infrastruction.Shared;

namespace KrepParser.Infrastruction.ParseConfiguration
{
    public sealed class SaturnParseConfiguration : BaseParseConfiguration, IParseConfiguration<ParseProductDTO>
    {
        private const string _ShopName = "Сатурн";

        private static string pageCountUrl = string.Empty;

        public SaturnParseConfiguration
            (string url, string request)
        {
            URL = url;
            Request = request;

            pageCountUrl = $"{url}catalog/?sp%25=&search=&s={request}&page=1";
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

                var content = await response.Content.ReadAsStringAsync();
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(req => req.Content(content));

                var result = document.QuerySelectorAll(".pagination > li");

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

            //Получение данных о ценах и названиях.
            using (var client = new HttpClient())
            {
                //Результат парсинга.
                var parseResult = new List<ParseProductDTO>();
                //Листы названий и цен.
                var names = new List<IHtmlCollection<IElement>>();
                var prices = new List<List<string>>();
                //Лист для записи корректной цены.
                var correctPrices = new List<List<double>>();

                for (int i = 1; i <= pageCount; i++)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}catalog/?sp%25=&search=&s={Request}&page={i}");
                    var response = await client.SendAsync(request);

                    if (response == null)
                        return ParseResult<List<ParseProductDTO>>
                            .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.ResponseIsNull));

                    var content = await response.Content.ReadAsStreamAsync();

                    var config = Configuration.Default.WithDefaultLoader();
                    var context = BrowsingContext.New(config);
                    var document = await context.OpenAsync(req => req.Content(content));

                    var priceblocks = document.QuerySelectorAll("._goods_card-block-price").ToList();

                    var currentPrice = new List<string>();

                    for (int element = 0; element < priceblocks.Count; element++)
                    {
                        var divs = priceblocks[element].ChildNodes.Where(i => i.NodeName == "DIV").ToList();

                        var bottomPriceElement = divs.Select(i => (IHtmlDivElement)i)
                            .Where(i => i.ClassName == "block-price-value-card").FirstOrDefault();
                        var topPriceElement = divs.Select(i => (IHtmlDivElement)i)
                            .Where(i => i.ClassName == "block-price-value").FirstOrDefault();

                        var currentBottomPrice = bottomPriceElement == null ? null
                            : bottomPriceElement.Attributes.Where(i => i.Name == "data-price").FirstOrDefault();
                        var currentTopPrice = topPriceElement == null ? null
                            : topPriceElement.Attributes.Where(i => i.Name == "data-price").FirstOrDefault();

                        currentPrice.Add(currentBottomPrice == null ? currentTopPrice.TextContent : currentBottomPrice.TextContent);
                    }

                    prices.Add(currentPrice);
                    names.Add(document.QuerySelectorAll(".goods-name"));

                    if ((prices.Count == 0 || names == null) || (names.Count == 0 || prices == null))
                        return ParseResult<List<ParseProductDTO>>
                        .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.SelectionIsEmpty));
                }

                //Запись цен.
                for (int i = 0; i < pageCount; i++)
                {
                    correctPrices.Add(GetPrice(prices[i]));
                }

                //Проверка цены.
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

        //Приведение цены в порядок.
        private List<double> GetPrice(List<string> price)
        {
            List<double> tempPrice = new();

            for (int i = 0; i < price.Count; i++)
            {
                double current = 0;

                if (price[i].Contains('.'))
                {
                    price[i] = price[i].Replace('.', ',');
                    current = double.TryParse(price[i], out double value) == true ? Convert.ToDouble(price[i]) : -1;
                }
                else
                    current = Convert.ToDouble(price[i]);

                tempPrice.Add(current);
            }

            return tempPrice;
        }
    }
}