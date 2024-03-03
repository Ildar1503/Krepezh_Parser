using KrepParser.Domain.Shared;

namespace KrepParser.Domain.Primitives
{
    public interface IBaseProductParse<ParseT> where ParseT : Result
    {
        public string ResourceName { get; set; }

        public Task<ParseT> ParseByName(string name);
        public Task<ParseT> ParseByDescriptionContains(string[] parametrs);
    }
}
