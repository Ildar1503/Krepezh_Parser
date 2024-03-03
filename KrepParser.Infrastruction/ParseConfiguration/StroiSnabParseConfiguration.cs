using AngleSharp;
using KrepParser.Application.DTO;
using KrepParser.Domain.Shared;
using KrepParser.Infrastruction.ParseConfiguration.Primitives;
using KrepParser.Domain.Enums;
using AngleSharp.Html.Dom;
using KrepParser.Infrastruction.Shared;

namespace KrepParser.Infrastruction.ParseConfiguration
{
    public sealed class StroiSnabParseConfiguration : BaseParseConfiguration, IParseConfiguration<ParseProductDTO>
    {
        private const string _ShopName = "Стройснаб";

        private static string pageCountUrl = string.Empty;

        public StroiSnabParseConfiguration
            (string url, string request)
        {
            URL = url;
            Request = request;

            pageCountUrl = $"{url}/search?search={request}";
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

                var result = document.QuerySelectorAll(".text_body > a");

                if (result.Length == 0 || result == null)
                    return ParseResult<int>
                        .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.SelectionIsEmpty));

                pageCount = result.Length >= 1 ? Convert.ToInt32(result[result.Length - 1].TextContent) : 1;
            }

            return ParseResult<int>.Succes(pageCount);
        }

        public async Task<ParseResult<List<ParseProductDTO>>> GetParseResultAsync()
        {
            //Лист для записи ссылок.
            var links = new List<string>();
            //Названия.
            var names = new List<string>();
            //Стоимсоть.
            var prices = new List<double>();
            //Итоговый.
            var parseResult = new List<ParseProductDTO>();

            //Колличество страниц.
            int pageCount = GetPageCountAsync().Result.Error == ParseError.Ok ?
                    GetPageCountAsync().Result.TValue : -1;

            //Проверка колличества страниц
            if (pageCount == -1)
                return ParseResult<List<ParseProductDTO>>
                    .FailureWithotValue(new ParseError(ParseErrors.ParseResultNullOrEmpty, ParseErrorsMessage.CannotFoundPageCount));


            //Получение всех используемых ссылок.
            using (var client = new HttpClient())
            {
                for (int i = 0; i < pageCount; i++)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}/search?search={Request}&p={i}");
                    var response = await client.SendAsync(request);

                    if (response == null)
                        return ParseResult<List<ParseProductDTO>>
                            .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.ResponseIsNull));

                    var content = await response.Content.ReadAsStreamAsync();
                    var config = Configuration.Default.WithDefaultLoader();
                    var context = BrowsingContext.New(config);
                    var document = await context.OpenAsync(req => req.Content(content));

                    var result = document.QuerySelectorAll(".text_body > ul > li > a");

                    if (result.Length == 0 || result == null)
                        return ParseResult<List<ParseProductDTO>>
                            .FailureWithotValue(new ParseError(ParseErrors.ParseResultNullOrEmpty, ParseErrorsMessage.CannotFoundLinks));

                    var tempListLinks = result.Select(x => ((IHtmlAnchorElement)x).Href).ToList();

                    for (int link = 0; link < tempListLinks.Count; link++)
                    {
                        int index = tempListLinks[link].IndexOf("localhost");
                        links.Add(tempListLinks[link].Remove(0, index + "localhost".Length));
                    }
                }
            }

            //Запись данных в листы.
            using (var client = new HttpClient())
            {
                for (int i = 0; i < links.Count; i++)
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}/{links[i]}");
                    var response = await client.SendAsync(request);

                    if (response == null)
                        return ParseResult<List<ParseProductDTO>>
                            .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.ResponseIsNull));

                    var content = await response.Content.ReadAsStreamAsync();
                    var config = Configuration.Default.WithDefaultLoader();
                    var context = BrowsingContext.New(config);
                    var document = await context.OpenAsync(req => req.Content(content));

                    var price = document.QuerySelector(".product-price > span").TextContent!;
                    var name = document.QuerySelector("h1").TextContent;

                    if ((name.Length == 0 || name == null) || (price.Length == 0 || price == null))
                        return ParseResult<List<ParseProductDTO>>
                            .FailureWithotValue(new ParseError(ParseErrors.ResponseNull, ParseErrorsMessage.SelectionIsEmpty));

                    prices.Add(double.TryParse(price, out double value) == true ? Convert.ToDouble(price) : -1);
                    names.Add(name);
                }
            }

            //Запись в итоговый лист
            for (int i = 0; i < names.Count; i++)
            {
                parseResult.Add(ParseProductDTO
                    .Create(names[i], prices[i], _ShopName));
            }

            return ParseResult<List<ParseProductDTO>>
                .Succes(parseResult);
        }
    }
}
