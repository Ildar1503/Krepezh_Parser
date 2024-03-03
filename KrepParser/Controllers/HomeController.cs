using KrepParser.Application.Commands.ProductsCRUD;
using KrepParser.Application.DTO;
using KrepParser.Application.Queries;
using KrepParser.Application.Services.Interfaces;
using KrepParser.Infrastruction.Services.Primitives;
using KrepParser.Infrastruction.Shared;
using Microsoft.AspNetCore.Mvc;
using static KrepParser.Infrastruction.InfrastructureServiceRegestration;

namespace KrepParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductAppService _productAppService;
        private readonly IWorkWithText _workWithText;
        private IParseService? _parseService;
        private string table = "";
        
        private ParseServiceResolver? parseServiceResolver = default;

        public HomeController
            (ParseServiceResolver? parseServiceResolver, 
            IProductAppService productAppService,
            IWorkWithText workWithText)
        {
            this.parseServiceResolver = parseServiceResolver;
            _productAppService = productAppService;
            _workWithText = workWithText;
        }

        public IActionResult HomeIndex()
        {
            return View();
        }
        
        [HttpGet]
        [Route("/GetTable&shopName={shopName}&request={request}")]
        public async Task<string> GetTable(string shopName, string request, CancellationToken cancellationToken)
        {
            _parseService = parseServiceResolver.Invoke(shopName);

            if (_parseService == null)
                return ParseErrorsMessage.InvalidMonitoring;

            string url = BaseUrls.GetCurrentUrl(shopName);
            //Инициализация экземпляра конфигурации парсера.
            _parseService.Create(url, request);

            //Получение спаршенных товаров.
            List<ParseProductDTO> parseProducts = new();
            try
            {
                parseProducts = await _parseService.GetProductsInformationAsync();
            }
            catch (Exception)
            {
                return ParseErrorsMessage.ResponseIsNull;
            }

            string table = _workWithText.GetTable(parseProducts);

            for (int i = 0; i < parseProducts.Count; i++)
            {
                var product = await _productAppService.GetProductByNameAsync
                    (new GetProductsByNameQuery() { Name = parseProducts[i].Name , ShopName = parseProducts[i].ShopName }, cancellationToken);

                if (product.TValue.Name.Length < 1 
                    && product.TValue.ShopName.Length < 1)
                {
                    var productDto = new ProductDTO
                    {
                        Id = Guid.NewGuid(),
                        Price = parseProducts[i].Price,
                        Name = parseProducts[i].Name,
                        ShopName = parseProducts[i].ShopName
                    };

                    var command = new AddProductCommand() { ProductDTO = productDto };

                    var result = await _productAppService.AddProductAsync(command, cancellationToken);

                    if (result.IsFailure)
                    {
                        return ParseErrorsMessage.DataTableIsEmptyOrNull;
                    }
                }
            }

            return table;
        }
    }
}
