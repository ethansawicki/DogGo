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
            Doggo doggo = _doggoRepo.GetDoggoById(id);

            if(doggo == null)
            {
                return NotFound();
            }
            return View(doggo);
        }

        // GET: DoggoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DoggoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Doggo doggo)
        {
            try
            {
                _doggoRepo.AddDoggo(doggo);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(doggo);
            }
        }

        // GET: DoggoController/Edit/5
        public ActionResult Edit(int id)
        {
            Doggo doggo = _doggoRepo.GetDoggoById(id);
            if(doggo == null)
            {
                return NotFound();
            }
            return View(doggo);
        }

        // POST: DoggoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Doggo doggo)
        {
            try
            {
                _doggoRepo.UpdateDoggo(doggo);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(doggo);
            }
        }

        // GET: DoggoController/Delete/5
        public ActionResult Delete(int id)
        {
            Doggo doggo = _doggoRepo.GetDoggoById(id);
            return View(doggo);
        }

        // POST: DoggoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Doggo doggo)
        {
            try
            {
                _doggoRepo.DeleteDoggo(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(doggo);
            }
        }
    }
}
