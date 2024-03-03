namespace KrepParser.Infrastruction.Shared
{
    /// <summary>
    /// Класс для записи базового url.
    /// </summary>
    public class BaseUrls
    {
        public static string Levsha { get; private set; } = "https://www.xn--80adix1e.xn--p1ai/catalog/";
        public static string StroiSnab { get; private set; } = "https://xn----7sbbaczvdqgnel1ahhdekje.xn--p1ai/";
        public static string Saturn { get; private set; } = "https://str.saturn.net/";

        public static string GetCurrentUrl(string shopName)
        {
            switch (shopName)
            {
                case "levsha":
                    return Levsha;
                case "stroi_snab":
                    return StroiSnab;
                case "saturn":
                    return Saturn;
                default:
                    return "Ресурс не найден.";
            }
        }
    }
}
