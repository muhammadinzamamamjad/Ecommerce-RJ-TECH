using Microsoft.AspNetCore.Mvc;
using RJTECH_Authentication_.Models;
using System.Diagnostics;

namespace RJTECH_Authentication_.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ProductRepository productRepository = new ProductRepository();
            List<Product> sm=productRepository.get("SmartWatches");
            List<Product> ap=productRepository.get("Airpods");

            int i = 0;
            List<Product> products = new List<Product>();
            foreach (Product p in sm)
            {
                if(i<=3)
                {
                    products.Add(p);
                }
                i++;
            }
            i = 0;
            foreach (Product p in ap)
            {
                if (i <= 3)
                {
                    products.Add(p);
                }
                i++;
            }
            ViewBag.Title = "Home";
            return View(products);
        }
        [HttpPost]
        public ActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return View(new List<Product>());
            }
            ProductRepository productRepository = new ProductRepository();

            IEnumerable<Product> Products = productRepository.GetAll();
            var filteredProducts = Products
                .Where(p => p.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            return View(filteredProducts);
        }
        public IActionResult Specific(int ID)
        {
            ProductRepository productsRepository = new ProductRepository();
            Product products = productsRepository.GetItem(ID);
            return View(products);
        }

    }
}
