using KrepParser.Application.DTO;

namespace KrepParser.Application.Queries.Primitives
{
    public abstract record BaseProductQuery
    {
        public string Name { get; set; }
        public string ShopName { get; set; }
    }
}
