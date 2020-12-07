using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _context;
        private readonly ILogger<HomeController> _logger;


        public MoviesController(MoviesContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Movies
        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Movies.Include(m => m.ActorGroup).ThenInclude(am => am.Actor).Select(m =>
                new MovieViewModel
                {
                    Id = m.Id,
                    Genre = m.Genre,
                    Price = m.Price,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Actors = m.ActorGroup.Select(af => af.Actor).ToList()
                }).ToList());
        }

        // GET: Movies/Details/5
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var viewModel = _context.Movies.Where(m => m.Id == id).Include(m => m.ActorGroup)
                .ThenInclude(am => am.Actor).Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Genre = m.Genre,
                    Price = m.Price,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Actors = m.ActorGroup.Select(af => af.Actor).ToList()
                }).FirstOrDefault();


            if (viewModel == null) return NotFound();

            return View(viewModel);
        }

        // GET: Movies/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,ReleaseDate,Genre,Price,ActorGroup")]
            InputMovieViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Genre = inputModel.Genre,
                    Price = inputModel.Price,
                    Title = inputModel.Title,
                    ReleaseDate = inputModel.ReleaseDate
                };
                _context.Add(movie);
                _context.SaveChanges();
                movie.ActorGroup =
                    inputModel.Actors.Select(a => new ActorsMovies() {MovieId = movie.Id, ActorId = a.Id}).ToList();
                _context.SaveChanges();


                return RedirectToAction(nameof(Index));
            }

            return View(inputModel);
        }

        [HttpGet]
        // GET: Movies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var editModel = _context.Movies.Where(m => m.Id == id).Include(m => m.ActorGroup)
                .ThenInclude(am => am.Actor).Select(m => new EditMovieViewModel
                {
                    Genre = m.Genre,
                    Price = m.Price,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Actors = m.ActorGroup.Select(af => af.Actor).ToList()
                }).FirstOrDefault();

            if (editModel == null) return NotFound();

            return View(editModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Title,ReleaseDate,Genre,Price,ActorGroup")]
            EditMovieViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var movie = new Movie
                    {
                        Id = id,
                        Genre = editModel.Genre,
                        Price = editModel.Price,
                        Title = editModel.Title,
                        ReleaseDate = editModel.ReleaseDate,
                        ActorGroup =
                            editModel.Actors.Select(a => new ActorsMovies() {MovieId = id, ActorId = a.Id}).ToList()
                    };

                    _context.Update(movie);
                    _context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (!MovieExists(id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(editModel);
        }

        [HttpGet]
        // GET: Movies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var deleteModel = _context.Movies.Where(m => m.Id == id).Include(m => m.ActorGroup)
                .ThenInclude(am => am.Actor).Select(m => new DeleteMovieViewModel
                {
                    Genre = m.Genre,
                    Price = m.Price,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    Actors = m.ActorGroup.Select(af => af.Actor).ToList()
                }).FirstOrDefault();

            if (deleteModel == null) return NotFound();

            return View(deleteModel);
        }

        // POST: Movies/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            _logger.LogError($"Movie with id {movie.Id} has been deleted!");
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}