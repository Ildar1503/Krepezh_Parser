using KrepParser.Application.DTO;

namespace KrepParser.Application.Services.Interfaces
{
    public interface IWorkWithText
    {
        //Формирует строку html таблицы.
        public string GetTable(List<ParseProductDTO> list);
    }
}
