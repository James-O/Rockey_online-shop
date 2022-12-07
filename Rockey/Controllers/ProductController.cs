using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Rockey.Data;
using Rockey.Models;
using Rockey.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Rockey.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> product = _db.Product.Include(a => a.Category).Include(a => a.ApplicationType);//Eager loading
            //foreach(var item in product)
            //{
            //    item.Category = _db.Category.FirstOrDefault(a => a.Id == item.CategoryId);
            //    item.ApplicationType = _db.ApplicationType.FirstOrDefault(a => a.Id == item.ApplicationTypeId);
            //}
            return View(product);
        }
        //GET Upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});
            ////pass the dropdown to view so that we can display it
            //ViewBag.CategoryDropDown = CategoryDropDown;

            //Product product = new Product();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is create
                return View(productVM);
            }
            else
            {
                //this update, then find the id
                productVM.Product = _db.Product.Find(id);
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            //if (ModelState.IsValid)
            //{
                var files = HttpContext.Request.Form.Files;//retrive new posted images
                string webRootPath = _webHostEnvironment.WebRootPath;// path to our www.root folder

                if(productVM.Product.Id == 0)//if no image,create else update
                {
                    //create
                    string upload = webRootPath+WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);//extension of uploaded file
                    //copy to new location - upload
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.Image = fileName + extension;//save the image here

                    //add the product
                    _db.Product.Add(productVM.Product);

                }
                else
                {
                    //updating
                    var itemFromDb = _db.Product.AsNoTracking().FirstOrDefault(a => a.Id == productVM.Product.Id);//get file from db on this id
                    if(files.Count > 0)//if any file exist on fend
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);//extension of uploaded file

                        var oldFile = Path.Combine(upload, itemFromDb.Image); //oldfile

                        if (System.IO.File.Exists(oldFile))//if old file exist
                        {
                            System.IO.File.Delete(oldFile);//remove it
                        }
                        //add new image ... copy to new location - upload 
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productVM.Product.Image = fileName + extension;//save the image here
                    }
                    else
                    {
                        productVM.Product.Image = itemFromDb.Image;//image is same if not modified
                    }
                _db.Product.Update(productVM.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");

            //valid state
            //}
            //productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //}),
            //productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});
            //return View(productVM);
        }
        //Delete Get
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            //var product = _db.Product.Find(id);
            //product.Category = _db.Category.Find(product.CategoryId);
            var product = _db.Product.Include(a => a.Category).Include(a=>a.ApplicationType).FirstOrDefault(a => a.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product );
        }
        //Delete Post
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var del = _db.Product.Find(id);
            if (del == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
   
            var oldFile = Path.Combine(upload, del.Image); //oldfile

            if (System.IO.File.Exists(oldFile))//if old file exist
            {
                System.IO.File.Delete(oldFile);//remove it
            }
            
            _db.Product.Remove(del);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
