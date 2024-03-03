namespace KrepParser.Application.DTO
{
    public class ParseProductDTO : EntityDTO
    {
        private ParseProductDTO(string name, double price, string shopName)
        {
            Name = name;
            Price = price;
            ShopName = shopName;
        }

        private ParseProductDTO(string name, string description, double price, double assessment, string shopName)
        {
            Name = name;
            Description = description;
            Price = price;
            Assessment = assessment;
            ShopName = shopName;
        }

        public string Name { get; private set;}
        public string? Description { get; private set;}
        public double Price { get; private set;}
        public double Assessment { get; private set;}
        //Название магазина.
        public string ShopName { get; private set;} 

        public static ParseProductDTO Create(string name, double price, string shopName) =>
            new ParseProductDTO(name, price, shopName);

        public static ParseProductDTO Create(string name, string description, double price, double assessment, string shopName) =>
            new ParseProductDTO(name, description, price, assessment, shopName);
    }
}
