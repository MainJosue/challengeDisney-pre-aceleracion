using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Model.Genero
{
    public class GeneroRequestModel
    {
        [Required]
        public string Nombre { get; set; }
        public string Imagen { get; set; }
      
    }
}
