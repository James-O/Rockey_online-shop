using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;


namespace Rockey.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; }
    }
}
