namespace KrepParser.Infrastruction.Shared
{
    public static class ParseErrorsMessage
    {
        public const string ResponseIsNull = "Результат запроса пуст.";
        public const string DataTableIsEmptyOrNull = "Не удалось добавить данные в таблицу.";
        public const string InvalidMonitoring = "Мониторинг ресурса недоступен.";
        public const string SelectionIsEmpty = "Результат поиска пуст.";
        public const string CannotFoundPageCount = "Не удалось получить колличество страниц";
        public const string InvalidPriceConvert = "Не получилось привести цены к корректному виду";
        public const string CannotFoundLinks = "Не удалось получить ссылки на страницы ресурса";
    }
}
