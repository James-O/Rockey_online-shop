using Microsoft.AspNetCore.Mvc;
using Rockey.Data;
using Rockey.Models;

namespace Rockey.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> appType = _db.ApplicationType;
            return View(appType);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType application)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(application);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }
        //Edit  
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var appType = _db.ApplicationType.Find(id);
            if(appType == null)
            {
                return NotFound();
            }
            return View(appType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType aptype)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(aptype);
                _db.SaveChanges();
                return RedirectToAction("Index");
            } 
            return View(aptype);
        }
        //Delete
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var apdel=_db.ApplicationType.Find(id);
            if (apdel == null)
            {
                return NotFound();
            }
            return View(apdel);
        }
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            var apdel = _db.ApplicationType.Find(id);
            if (apdel == null)
            {
                return NotFound();
            }
            _db.ApplicationType.Remove(apdel);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
