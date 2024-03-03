using KrepParser.Infrastruction.ParseConfiguration.Primitives;

namespace KrepParser.Infrastruction.Services.Primitives
{
    /// <summary>
    /// Обязательное свойство типа конфигуратора парсера.
    /// </summary>
    public interface IParseConfiguartion<ParseTypeConfiguration> 
        where ParseTypeConfiguration : BaseParseConfiguration
    {
        public ParseTypeConfiguration CurrentParseTypeConfiguration { get; set; } 
    }
}
