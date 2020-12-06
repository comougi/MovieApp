using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class ActorsMovies
    {
        [ForeignKey("Actor")] public int ActorId { get; set; }
        [ForeignKey("Movie")] public int MovieId { get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }
        
    }
}