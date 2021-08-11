using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeDisney.Entidades;

namespace ChallengeDisney.Model.Personaje
{
    public class PersonajeResponseModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public int Año { get; set; }
        public float Peso { get; set; }
        public string Historia { get; set; }
        public int idPeliculas { get; set; }
    }
}
