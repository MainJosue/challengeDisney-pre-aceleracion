using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Entidades
{
    public class Generos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public ICollection<Peliculas> Peliculas { get; set; } = new List<Peliculas>();
    }
}
