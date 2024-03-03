using KrepParser.Application.DTO;

namespace KrepParser.Infrastruction.Services.Primitives
{
    public interface IParseService
    {
        //Получение HTML таблицы.
        public Task<List<ParseProductDTO>> GetProductsInformationAsync();
        //Получение таблицы Excel.
        public Task<string> GetExcelTableAsync();
        //Заполнение свойства экземпляра типа конфигурации. 
        public void Create(string url, string request);
    }
}
