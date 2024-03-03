using KrepParser.Domain.Primitives;

namespace KrepParser.Domain.Entites
{
    public sealed class Product : AggregateRoot
    {
        private Product()
        {
            Name = string.Empty;
            ShopName = string.Empty;
        }

        private Product(
            Guid id, 
            string name, 
            double price, 
            string shopName)
            :base(id)
        {
            Name = name;
            Price = price;
            Date = DateTime.Now;
            ShopName = shopName;
        }
            
        public string Name { get; private set; }
        public double Price { get; private set; }
        //Дата загрузки.
        public DateTime Date { get; private set; }
        //Название магазина.
        public string ShopName { get; private set; }

        public static Product CreateProduct(
            Guid id,
            string name,
            double price,
            string shopName)
        {
            Product product = new(
                id,
                name,
                price,
                shopName)
            {
                Name = name,
                Price = price,
                ShopName = shopName
            };

            return product;
        }

        public static Product GetEmptyProduct()
        {
            return new Product();
        }
    }
}

