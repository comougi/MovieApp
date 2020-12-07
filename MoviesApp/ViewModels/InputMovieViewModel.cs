using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class InputMovieViewModel
    {
        public string Title { get; set; }

        [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }
        public decimal Price { get; set; }

        public List<Actor> Actors { get; set; }
    }
}