namespace Rockey.Models.ViewModel
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }
        public IUser IUser { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
    }
}
