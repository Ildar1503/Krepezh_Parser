namespace KrepParser.Infrastruction.Shared
{
    public sealed class ShopName
    {
        public static string GetShopName(string name)
        {
            switch (name)
            {
                case "levsha":
                    return "Левша";
                case "stroi_snab":
                    return "Стройснаб";
                case "saturn":
                    return "Сатурн";
                default:
                    return "Не найдено";
            }
        }
    }
}
