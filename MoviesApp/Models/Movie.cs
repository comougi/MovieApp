using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
    public class Movie
    {
        public Movie()
        {
        }

        public Movie(ICollection<ActorsMovies> actorGroup)
        {
            ActorGroup = new HashSet<ActorsMovies>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<ActorsMovies> ActorGroup { get; set; }
    }
}