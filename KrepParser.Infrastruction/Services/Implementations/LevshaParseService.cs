using KrepParser.Application.DTO;
using KrepParser.Infrastruction.ParseConfiguration;
using KrepParser.Infrastruction.Services.Primitives;

namespace KrepParser.Infrastruction.Services.Implementations
{
    public sealed class LevshaParseService : IParseService, IParseConfiguartion<LevshaParseConfiguration>
    {
        public LevshaParseConfiguration CurrentParseTypeConfiguration { get; set; } = default!;

        public void Create(string url, string request)
        {
            CurrentParseTypeConfiguration = new LevshaParseConfiguration(url, request);
        }

        //TODO: подумать над реализацией.
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
