using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Model.Pelicula
{
    public class PeliculaRequestModel
    {
        
        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }
        public string Imagen { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [Range(1, 5)]
        public int Calificacion { get; set; }
        [Required]
        public int idGenero { get; set; }
        public int idPersonaje { get; set; }

    }
}
