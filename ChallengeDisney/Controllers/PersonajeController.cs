using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallengeDisney.Context;
using ChallengeDisney.Entidades;
using ChallengeDisney.Model.Personaje;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeDisney.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonajeController : ControllerBase
    {
        private readonly ChallengeDisneyContext _context;

        public PersonajeController(ChallengeDisneyContext ctx)
        {
            _context = ctx;
        }

        

        [HttpGet]
        [Route("characters")]
        public IActionResult GetPersonaje(int idPelicula, int año, string nombre, float peso)
        {
            IQueryable<Personajes> list = _context.Personajes.Include(x => x.Peliculas);

            if (idPelicula != 0) list = list.Where(x => x.Peliculas.FirstOrDefault(x => x.Id == idPelicula) != null);

            if (año != 0) list = list.Where(x => x.Año == año);

            if (nombre != null) list = list.Where(x => x.Nombre == nombre);

            if (peso != 0) list = list.Where(x => x.Peso == peso);

            return Ok(list
                .Select(x => new { Nombre = x.Nombre, Imagen = x.Imagen })
                .ToList());
        }

        [HttpGet]
        [Route("characters/{id}")]
        public IActionResult GetPersonaje(int id)
        {
            var personaje = _context.Personajes
                .Include(x => x.Peliculas)
                .FirstOrDefault(x => x.Id == id);

            if (personaje == null)
            {
                return NotFound("El ID ingresado no existe.");
            }

            return Ok(personaje);
           
        }

        [HttpPost]
        [Route("agregarPersonaje")]
        public IActionResult Post(PersonajeRequestModel personaje)
        {

            var img = "";
           
            var hist = "";

            if(personaje.Imagen != "")
            {
                img = personaje.Imagen;
            }

            if(personaje.Historia != "")
            {
                hist = personaje.Historia;
            }


            var nuevopersonaje = new Personajes
            {
                Nombre = personaje.Nombre,
                Imagen = img,
                Año = personaje.Año,
                Peso = personaje.Peso,
                Historia = hist
            };

            if (personaje.idPelicula != 0)
            {
                var pelicula = _context.Peliculas.FirstOrDefault(x => x.Id == personaje.idPelicula);

                if (pelicula != null)
                {                   
                    nuevopersonaje.Peliculas.Add(pelicula);                     
                }
            }

            _context.Personajes.Add(nuevopersonaje);

            _context.SaveChanges();

            return Ok(new PersonajeResponseModel
            {
                Id = nuevopersonaje.Id,
                Nombre = nuevopersonaje.Nombre,
                Imagen = nuevopersonaje.Imagen,
                Año = nuevopersonaje.Año,
                Peso = nuevopersonaje.Peso,
                Historia = nuevopersonaje.Historia,
                idPeliculas = personaje.idPelicula
            });
        }
        [HttpPost]
        [Route("agregarPelicula")]
        public IActionResult Post( int idPersonaje,int idPelicula)
        {
            var personaje = _context.Personajes.FirstOrDefault(x => x.Id == idPersonaje);

            if (personaje == null)
            {
                return NotFound("El Personaje ingregasada NO EXISTE");
            }           
            
           
            var pelicula = _context.Peliculas.FirstOrDefault(x => x.Id == idPelicula);
            if (pelicula == null)
            {
                return NotFound("La Pelicula ingregasada NO EXISTE");
            }

            personaje.Peliculas.Add(pelicula);

            _context.Personajes.Update(personaje);

            _context.SaveChanges();

            return Ok();

        }
        [HttpPut]
        [Route("actualizarPersonaje")]
        public IActionResult Put(Personajes personaje)
        {
            var nuevoPersonajue = _context.Personajes.FirstOrDefault(x => x.Id == personaje.Id);

            if (nuevoPersonajue == null)
            {
                return NotFound("El Personaje ingresado NO EXISTE.");
            }

            var año = personaje.Año.ToString();

            if (personaje.Nombre == "" || año == "")
            {
                return NotFound("Falto algun campo requerido.");
            }

            var img = "";
            var hist = "";

            if (personaje.Imagen != "")
            {
                img = personaje.Imagen;
            }

            if (personaje.Historia != "")
            {
                hist = personaje.Historia;
            }

            nuevoPersonajue.Nombre = personaje.Nombre;
            nuevoPersonajue.Imagen = personaje.Imagen;
            nuevoPersonajue.Año = personaje.Año;
            nuevoPersonajue.Peso = personaje.Peso;
            nuevoPersonajue.Historia = personaje.Historia;

            _context.Personajes.Update(nuevoPersonajue);

            _context.SaveChanges();

            return Ok(nuevoPersonajue);
        }

        [HttpDelete]
        [Route("eliminarPersonaje/{idPersonaje}")]
        public IActionResult Delete(int idPersonaje)
        {
            var personaje = _context.Personajes.FirstOrDefault(x => x.Id == idPersonaje);

            if (personaje == null)
            {
                return NotFound("El personaje que desea eliminar no existe.");
            }

            _context.Personajes.Remove(personaje);

            _context.SaveChanges();

            return Ok(_context.Personajes.ToList());
        }
    }
}
