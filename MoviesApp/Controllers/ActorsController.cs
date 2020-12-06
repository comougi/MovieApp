using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class ActorsController : Controller
    {
        private readonly MoviesContext _context;
        private readonly ILogger<HomeController> _logger;


        public ActorsController(MoviesContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Actors.Include(af => af.ActorFilmography).ThenInclude(am => am.Movie).Select(a =>
                new ActorViewModel
                {
                    Id = a.Id,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname,
                    Birthdate = a.Birthdate,
                    ActorFilmography = a.ActorFilmography
                }).ToList());
        }

        
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = _context.Actors.Where(m => m.Id == id).Include(af => af.ActorFilmography)
                .ThenInclude(am => am.Movie).Select(a => new ActorViewModel
                {
                    Id = a.Id,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname,
                    Birthdate = a.Birthdate,
                    ActorFilmography = a.ActorFilmography
                }).FirstOrDefault();


            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Firstname,Lastname,Birthdate,ActorFilmography")]
            InputActorViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Actor
                {
                    Id = inputModel.Id,
                    Firstname = inputModel.Firstname,
                    Lastname = inputModel.Lastname,
                    Birthdate = inputModel.Birthdate,
                    ActorFilmography = inputModel.ActorFilmography
                });
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(inputModel);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var editModel = _context.Actors.Where(a => a.Id == id).Include(af => af.ActorFilmography)
                .ThenInclude(am => am.Movie).Select(a => new EditActorViewModel
                {
                    Id = a.Id,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname,
                    Birthdate = a.Birthdate,
                    ActorFilmography = a.ActorFilmography
                }).FirstOrDefault();

            if (editModel == null) return NotFound();

            return View(editModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Firstname,Lastname,Birthdate,ActorFilmography")]
            EditActorViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var actor = new Actor
                    {
                        Id = id,
                        Firstname = editModel.Firstname,
                        Lastname = editModel.Lastname,
                        Birthdate = editModel.Birthdate,
                        ActorFilmography = editModel.ActorFilmography
                    };

                    _context.Update(actor);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (!ActorExists(id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(editModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var deleteModel = _context.Actors.Where(a => a.Id == id).Include(af => af.ActorFilmography)
                .ThenInclude(am => am.Movie).Select(a => new DeleteActorViewModel
                {
                    Id = a.Id,
                    Firstname = a.Firstname,
                    Lastname = a.Lastname,
                    Birthdate = a.Birthdate,
                    ActorFilmography = a.ActorFilmography
                }).FirstOrDefault();

            if (deleteModel == null) return NotFound();

            return View(deleteModel);
        }

        
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var actor = _context.Actors.Find(id);
            _context.Actors.Remove(actor);
            _context.SaveChanges();
            _logger.LogError($"Actor with id {actor.Id} has been deleted!");
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}