namespace KrepParser.Application.DTO
{
    public sealed class ProductDTO : EntityDTO
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string ShopName { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now.Date;
    }
}
