using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Model.Pelicula
{
    public class PeliculaResponseModel
    {
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }
        public int idGenero { get; set; }
    }
}
