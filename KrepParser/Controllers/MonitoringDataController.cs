using KrepParser.Application.DTO;
using KrepParser.Application.Queries;
using KrepParser.Application.Services.Interfaces;
using KrepParser.Infrastruction.Shared;
using Microsoft.AspNetCore.Mvc;

namespace KrepParser.Controllers
{
    public class MonitoringDataController : Controller
    {
        private readonly IProductAppService _productAppService;
        private readonly IWorkWithText _workWithText;

        public MonitoringDataController(IProductAppService productAppService, IWorkWithText workWithText)
        {
            _productAppService = productAppService;
            _workWithText = workWithText;
        }

        public IActionResult MonitoringDataIndex()
        {
            return View();
        }

        [HttpGet]
        [Route("/GetInformationTable&shopName={shopName}&request={request}")]
        public async Task<string> GetInformationTable
            (string shopName, string request, CancellationToken cancellationToken)
        {
            var query = new GetCurrentShopProductsByNameQuery() { Name = request, ShopName = ShopName.GetShopName(shopName) };

            if (query is null)
                return "Запрос пуст";

            var products = await _productAppService.GetCurrentShopProductsByNameAsync(query, cancellationToken);

            if (products.IsFailure)
                return products.Error.description;

            var productsDTO = products.TValue.Select(product => ParseProductDTO
                .Create(product.Name, product.Price, product.ShopName)).ToList();

            return _workWithText.GetTable(productsDTO);
        }
    }
}
