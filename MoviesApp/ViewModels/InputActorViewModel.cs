using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoviesApp.Models;

namespace MoviesApp.ViewModels
{
    public class InputActorViewModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [DataType(DataType.Date)] public DateTime Birthdate { get; set; }

        
        public List<Movie> Movies { get; set; }
    }
}