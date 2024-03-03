using KrepParser.Application.DTO;
using KrepParser.Infrastruction.ParseConfiguration;
using KrepParser.Infrastruction.Services.Primitives;

namespace KrepParser.Infrastruction.Services.Implementations
{
    public sealed class SaturnParseService : IParseService, IParseConfiguartion<SaturnParseConfiguration>
    {
        public SaturnParseConfiguration CurrentParseTypeConfiguration { get; set; } = default!;

        public void Create(string url, string request)
        {
            CurrentParseTypeConfiguration = new SaturnParseConfiguration(url, request);
        }

        public Task<string> GetExcelTableAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ParseProductDTO>> GetProductsInformationAsync()
        {
            var parseResult = await CurrentParseTypeConfiguration.GetParseResultAsync();

            if (parseResult.IsFailure)
                return (new List<ParseProductDTO>());

            return parseResult.TValue!;
        }
    }
}
