using Microsoft.AspNetCore.Mvc;
using RJTECH_Authentication_.Models;

namespace RJTECH_Authentication_.Controllers
{
    public class WatchStrapsController : Controller
    {
        public IActionResult Index()
        {
            ProductRepository productsRepository = new ProductRepository();
            List<Product> products = productsRepository.get("WatchStraps");
            return View(products);
        }
        public IActionResult Specific(int Id)
        {
            ProductRepository productsRepository = new ProductRepository();
            Product products = productsRepository.GetItem(Id);
            return View(products);
        }

    }
}
