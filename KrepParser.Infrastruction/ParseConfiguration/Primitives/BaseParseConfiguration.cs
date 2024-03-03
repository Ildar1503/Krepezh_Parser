namespace KrepParser.Infrastruction.ParseConfiguration.Primitives
{
    public abstract class BaseParseConfiguration
    {
        //Url главной страницы.
        public string? URL { get; init; }
        //Запрос.
        public string Request { get; set; }
    }
}
