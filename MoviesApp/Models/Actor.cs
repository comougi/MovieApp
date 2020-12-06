using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
    public class Actor
    {
        public Actor(ICollection<ActorsMovies> actorFilmography)
        {
            ActorFilmography = new HashSet<ActorsMovies>();
        }

        public Actor()
        {
        }

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [DataType(DataType.Date)] public DateTime Birthdate { get; set; }

        public ICollection<ActorsMovies> ActorFilmography { get; set; }
    }
}