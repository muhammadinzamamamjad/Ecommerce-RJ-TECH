using Microsoft.AspNetCore.Mvc;
using RJTECH_Authentication_.Models;

namespace RJTECH_Authentication_.Controllers
{
    public class AirpodsController : Controller
    {
        public IActionResult Index()
        {
            ProductRepository productsRepository = new ProductRepository();
            List<Product> products = productsRepository.get("Airpods");
            return View(products);
        
        }
        public IActionResult Specific(int ID)
        {
            ProductRepository productsRepository = new ProductRepository();
            Product products = productsRepository.GetItem(ID);
            return View(products);
        }
    }
}
