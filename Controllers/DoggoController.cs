using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class DoggoController : Controller
    {
        private readonly IDoggoRepository _doggoRepo;

        public DoggoController(IDoggoRepository doggoRepo)
        {
            _doggoRepo = doggoRepo;
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        // GET: DoggoController
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Doggo> doggoList = _doggoRepo.GetDoggosByOwnerId(ownerId);

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
        [Authorize]
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
                doggo.Id = GetCurrentUserId();

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
            int ownerId = GetCurrentUserId();

            if(doggo.OwnerId != ownerId || doggo == null)
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
