using KrepParser.Application.DTO;
using KrepParser.Application.Services.Interfaces;

namespace KrepParser.Application.Services.Implementations
{
    public sealed class WorkWithText : IWorkWithText
    {
        public string GetTable(List<ParseProductDTO> list)
        {
            string table =
                "<table>" +
                "<thead>" +
                    "<tr>" +
                        "<th class=\"product_name_th\">Название</th>" +
                        "<th class=\"product_price_th\">Цена</th>" +
                        "<th class=\"product_price_th\">Магазин</th>" +
                    "</tr>" +
                "</thead>" +
                "<tbody>";

            foreach (var item in list)
            {
                table +=
                    "<tr>" +
                        $"<td class=\"product_name_td\">{item.Name}</td>" +
                        $"<td class=\"product_price_td\">{item.Price}</td>" +
                        $"<td class=\"product_shop_name_td\">{item.ShopName}</td>" +
                    "</tr>";
            }

            table +=
                "</tbody>" +
                "</table>";

            return table;
        }
    }
}
