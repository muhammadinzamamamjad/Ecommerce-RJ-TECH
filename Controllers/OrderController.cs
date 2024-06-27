using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RJTECH_Authentication_.Models;

namespace RJTECH_Authentication_.Controllers
{
    public class OrderController : Controller
    {
        static string connect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RJTECHDB;Integrated Security=True";

        [HttpPost]

        public IActionResult AddToCart(int Id, string Name, Decimal Price, Decimal Discprice, string Image, string Category, int Quantity)
        {
            ProductRepository productRepository = new ProductRepository();
            int availableStock = productRepository.GetStockQuantity(Id);

            // Check if the requested quantity is available
            if (Quantity > availableStock)
            {
                TempData["Error"] = "Requested quantity is not available in stock.";
                return RedirectToAction("Specific", Category, new { ID = Id });
            }
            if (User.Identity.IsAuthenticated)
            {
                CartItems cartItem = null;
                TempData["UserID"] = User.Identity.Name;
                CartRepository cartRepository = new CartRepository(connect);
                cartItem = cartRepository.GetItem(Id, User.Identity.Name);

                if (cartItem != null)
                {
                    cartItem.Quantity += Quantity;
                    cartRepository.Update(cartItem);
                }
                else
                {
                    // Create a new cart item and save it to the database
                    cartItem = new CartItems
                    {
                        Name = Name,
                        Price = Price,
                        Discprice = Discprice,
                        Image = Image,
                        Category = Category,
                        ProductId = Id,
                        Quantity = Quantity,
                        UserId = User.Identity.Name // Associate the cart item with the current user
                    };
                    cartRepository.Add(cartItem);

                }
            }
            else
            {
                CartItems cartItem = null;

                // If the user is not logged in, save the cart item to the session
                var cartItems = HttpContext.Session.Get<List<CartItems>>("CartProducts") ?? new List<CartItems>();

                var item = cartItems.SingleOrDefault(x => x.ProductId == Id);
                if (item != null)
                {
                    item.Quantity += Quantity;
                }
                else
                {
                    cartItem = new CartItems
                    {
                        Name = Name,

                        Price = Price,
                        Discprice = Discprice,
                        Image = Image,
                        Category = Category,
                        ProductId = Id,
                        Quantity = Quantity,
                    };
                    cartItems.Add(cartItem);
                }

                HttpContext.Session.Set("CartProducts", cartItems);

            }

            //productRepository.UpdateStockQuantity(Id, availableStock-Quantity);

            //bool cookieExists = HttpContext.Request.Cookies.ContainsKey("cartItemCount");
            // Update the cookie with the correct cart item count
            int currentCartItemCount = 0;
            if (HttpContext.Request.Cookies.TryGetValue("cartItemCount", out var cartItemCountStr))
            {
                currentCartItemCount = Convert.ToInt32(cartItemCountStr);
            }
            else
            {
                // Initialize the cartItemCount cookie if it does not exist
                currentCartItemCount = 0;
            }
            int newCartItemCount = currentCartItemCount + Quantity;
            HttpContext.Response.Cookies.Append("cartItemCount", newCartItemCount.ToString()
                //, new CookieOptions
                //{
                //    Expires = DateTime.Now.AddDays(7)
                //}
                ) ;

            // Redirect based on category
            return Category switch
            {
                "SmartWatches" => RedirectToAction("Specific", "SmartWatches", new {ID=Id}),
                "Airpods" => RedirectToAction("Specific", "Airpods", new { ID = Id }),
                "Handsfree" => RedirectToAction("Specific", "Handsfree", new { ID = Id }),
                "WatchStraps" => RedirectToAction("Specific", "WatchStraps", new { ID = Id }),
            };
        }

        public IActionResult ViewCart()
        {
            List<CartItems> cartItems = new List<CartItems>();

            if (User.Identity.IsAuthenticated) // Check if the user is authenticated
            {
                CartRepository cartRepository = new CartRepository(connect);
                cartItems = cartRepository.GetItemsByUser(User.Identity.Name);
            }
            else
            {
                // Fetch cart items from the session for anonymous users
                cartItems = HttpContext.Session.Get<List<CartItems>>("CartProducts") ?? new List<CartItems>();
            }
            if(cartItems.Count > 0)
            {
            return View(cartItems);
            }
            else
            {
                return View("EmptyCart");
            }

        }
        public IActionResult EmptyCart()
        {
            return View();
        }
        public IActionResult RemoveFromCart(int ID)
        {
            ProductRepository productRepository = new ProductRepository();
            int availableStock = productRepository.GetStockQuantity(ID);
            int releasequantity = 0;
            if (User.Identity.IsAuthenticated) // Check if the user is authenticated
            {
                // Remove from database
                CartRepository cartRepository = new CartRepository(connect);
                var cartItem = cartRepository.GetItem(ID, User.Identity.Name);
                releasequantity = cartItem.Quantity;
                if (cartItem != null)
                {
                    cartRepository.Remove(cartItem);
                }
            }
            else
            {
                // Remove from session
                var cartItems = HttpContext.Session.Get<List<CartItems>>("CartProducts") ?? new List<CartItems>();
                var item = cartItems.SingleOrDefault(x => x.ProductId == ID);
                releasequantity = item.Quantity;

                if (item != null)
                {
                    cartItems.Remove(item);
                    HttpContext.Session.Set("CartProducts", cartItems);
                }
            }

            //productRepository.UpdateStockQuantity(ID, availableStock + releasequantity);


            // Redirect to the viewCart action
            return RedirectToAction("ViewCart");
        }
    }
}
