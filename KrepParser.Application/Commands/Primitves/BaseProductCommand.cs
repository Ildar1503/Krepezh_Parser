using KrepParser.Application.DTO;

namespace KrepParser.Application.Commands.Primitves
{
    public abstract record BaseProductCommand
    {
        public ProductDTO ProductDTO { get; set; }
    }
}
