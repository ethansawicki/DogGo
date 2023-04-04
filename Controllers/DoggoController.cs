using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class DoggoController : Controller
    {
        private readonly IDoggoRepository _doggoRepo;

        public DoggoController(IDoggoRepository doggoRepo)
        {
            _doggoRepo = doggoRepo;
        }


        // GET: DoggoController
        public ActionResult Index()
        {
            List<Doggo> doggoList = _doggoRepo.GetAllDoggos();
            return View(doggoList);
        }

        // GET: DoggoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DoggoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DoggoController/Create
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

        // GET: DoggoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DoggoController/Edit/5
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

        // GET: DoggoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DoggoController/Delete/5
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
}
