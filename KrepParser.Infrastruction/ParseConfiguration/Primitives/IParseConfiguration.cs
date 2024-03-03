using KrepParser.Application.DTO;
using KrepParser.Domain.Shared;

namespace KrepParser.Infrastruction.ParseConfiguration.Primitives
{
    public interface IParseConfiguration<T> where T : ParseProductDTO
    {
        //Получение колличества страниц.
        public Task<ParseResult<int>> GetPageCountAsync();
        //Получение результата парсинга.
        public Task<ParseResult<List<T>>> GetParseResultAsync();
    }
}
