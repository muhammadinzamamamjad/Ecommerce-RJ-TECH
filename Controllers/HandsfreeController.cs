using Microsoft.AspNetCore.Mvc;
using RJTECH_Authentication_.Models;

namespace RJTECH_Authentication_.Controllers
{
    public class HandsfreeController : Controller
    {
        public IActionResult Index()
        {
            ProductRepository productsRepository = new ProductRepository();
            List<Product> products = productsRepository.get("Handsfree");
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
