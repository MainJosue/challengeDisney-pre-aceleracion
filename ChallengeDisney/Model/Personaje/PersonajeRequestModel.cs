using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Model.Personaje
{
    public class PersonajeRequestModel
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        [Required]
        public int Año { get; set; }
        public float Peso { get; set; }
        
        public string Historia { get; set; }
        
        public int idPelicula { get; set; }

    }
}
