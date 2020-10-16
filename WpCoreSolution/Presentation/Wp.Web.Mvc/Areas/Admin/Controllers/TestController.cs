using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestController : Controller
    {
        private List<TestModel> models;
        public TestController()
        {
            models = new List<TestModel>();
            models.Add(new TestModel { Id = 1, Name = "Test 1" });
            models.Add(new TestModel { Id = 2, Name = "Test 2" });
            models.Add(new TestModel { Id = 3, Name = "Test 3" });
            models.Add(new TestModel { Id = 4, Name = "Test 4" });
        }
        // GET: TestController
        public ActionResult Index()
        {

            return View(models);
        }

        // GET: TestController/Details/5
        public ActionResult Details(int id)
        {
            var model = models.First(x => x.Id == id);
            return View(model);
        }

        // GET: TestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = models.First(x => x.Id == id);
            return View(model);
        }

        // POST: TestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
