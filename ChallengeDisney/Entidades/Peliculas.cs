using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Entidades
{
    public class Peliculas
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }
        public Generos Genero { get; set; }
        public ICollection<Personajes> Personajes { get; set; } = new List<Personajes>();
    }
}
