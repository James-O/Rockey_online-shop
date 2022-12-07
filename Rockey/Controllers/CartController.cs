using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rockey.Data;
using Rockey.Models;
using Rockey.Models.ViewModel;
using Rockey.Utility;
using System.Security.Claims;

namespace Rockey.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> productInCart = shoppingCartList.Select(a=>a.ProductId).ToList();//lists of products in cart
            IEnumerable<Product> productList = _db.Product.Where(b=>productInCart.Contains(b.Id));//product list
            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity; //claims objt
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);//retrives user id when he registered
            //var userId = User.FindFirstValue(ClaimTypes.Name);//second way of retriving user identity

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> productInCart = shoppingCartList.Select(a => a.ProductId).ToList();//lists of products in cart
            IEnumerable<Product> productList = _db.Product.Where(b => productInCart.Contains(b.Id));//product list

            ProductUserVM = new ProductUserVM()
            {
                IUser = _db.IUser.FirstOrDefault(a => a.Id == claim.Value),
                ProductList = productList
            };
            return View(ProductUserVM);
        }
        public IActionResult Remove (int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(a => a.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
