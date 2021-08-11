using ChallengeDisney.Context;
using ChallengeDisney.Entidades;
using ChallengeDisney.Model.Pelicula;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ChallengeDisney.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PeliculaController :ControllerBase
    {
        private readonly ChallengeDisneyContext _context;

        public PeliculaController(ChallengeDisneyContext ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        [Route("movies")]
        public IActionResult GetPelicula(string titulo, int idGenero, string ordenar)
        {
            IQueryable<Peliculas> list = _context.Peliculas.Include(x => x.Personajes);

            if (titulo != null) list = list.Where(x => x.Titulo == titulo);

            if (idGenero != 0) list = list.Where(x => x.Genero.Id == idGenero);

            if (ordenar == "ASC") list = list.OrderBy(x => x.FechaCreacion);

            if (ordenar == "DESC") list = list.OrderByDescending(x => x.FechaCreacion);

            return Ok(list.Select(x => new
            {
                Imagen = x.Imagen,
                Titulo = x.Titulo,
                FechaCreacion = x.FechaCreacion
            }).ToList());
        }

        [HttpGet]
        [Route("movies/{idPelicula}")]
        public IActionResult GetPelicula(int idPelicula)
        {
            var pelicula = _context.Peliculas.Include(x => x.Personajes).Include(x => x.Genero).FirstOrDefault(x => x.Id == idPelicula);

            if (pelicula == null)
            {
                return NotFound("La Pelicula ingregasada NO EXISTE");
            }

            return Ok(pelicula);
        }

        [HttpPost]
        [Route("agregarPelicula")]
        public IActionResult Post(PeliculaRequestModel pelicula)
        {
            var img = "";
            if(pelicula.Imagen != "")
            {
                img = pelicula.Imagen;
            }

            var nuevaPelicula = new Peliculas
            {            
                Imagen = img,
                Titulo = pelicula.Titulo,
                FechaCreacion = pelicula.FechaCreacion,
                Calificacion = pelicula.Calificacion
            };

            if (pelicula.idGenero != 0)
            {
                var genero = _context.Generos.FirstOrDefault(x => x.Id == pelicula.idGenero);

                if (genero != null)
                {
                    if (nuevaPelicula.Genero== null) nuevaPelicula.Genero = new Generos();

                    nuevaPelicula.Genero = genero;
                }
            }

            if (pelicula.idPersonaje != 0)
            {
                var personaje = _context.Personajes.FirstOrDefault(x => x.Id == pelicula.idPersonaje);

                if (personaje != null)
                {
                    if (personaje == null) nuevaPelicula.Personajes = new List<Personajes>();

                    nuevaPelicula.Personajes.Add(personaje);
                }
            }

            _context.Peliculas.Add(nuevaPelicula);

            _context.SaveChanges();

            return Ok(new PeliculaResponseModel
            {
                Imagen = pelicula.Imagen,
                Titulo = pelicula.Titulo,
                FechaCreacion = pelicula.FechaCreacion,
                Calificacion = pelicula.Calificacion,
                idGenero = pelicula.idGenero

            });
        }

        [HttpPost]
        [Route("agregarPersonaje")]
        public IActionResult Post(int idPelicula, int idPersonaje)
        {

            var pelicula = _context.Peliculas.Include(x => x.Personajes).Include(x => x.Genero).FirstOrDefault(x => x.Id == idPelicula);

            if (pelicula == null)
            {
                return NotFound("La Pelicula ingregasada NO EXISTE");
            }

            var personaje = _context.Personajes.FirstOrDefault(x => x.Id == idPersonaje);

            if (personaje == null)
            {
                return NotFound("El Personaje ingregasada NO EXISTE");
            }

            pelicula.Personajes.Add(personaje);

            _context.Peliculas.Update(pelicula);

            _context.SaveChanges();

            return Ok();
           
        }

        [HttpPut]
        [Route("actualizarPelicula")]
        public IActionResult Put(Peliculas pelicula)
        {
            var nuevaPelicula = _context.Peliculas.FirstOrDefault(x => x.Id == pelicula.Id);

            if (nuevaPelicula == null)
            {
                return NotFound("La Pelicula ingregasada NO EXISTE.");
            }

            var img = "";           
            var fechac = pelicula.FechaCreacion.ToString();             

            if (pelicula.Calificacion == 0 ||  fechac == "" || pelicula.Titulo == "")
            {
                return NotFound("Falta algun campo requerido.");
            }

            if(pelicula.Imagen != "")
            {
                img = pelicula.Imagen;
            }

            nuevaPelicula.Imagen = img;
            nuevaPelicula.Titulo = pelicula.Titulo;
            nuevaPelicula.FechaCreacion = pelicula.FechaCreacion;
            nuevaPelicula.Calificacion = pelicula.Calificacion;

            _context.Peliculas.Update(nuevaPelicula);

            _context.SaveChanges();

            return Ok(nuevaPelicula);
        }


        [HttpDelete]
        [Route("eliminarPelicula/{idPelicula}")]
        public IActionResult Delete(int idPelicula)
        {
            var eliminarPelicula = _context.Peliculas.FirstOrDefault(x => x.Id == idPelicula);

            if (eliminarPelicula == null)
            {
                return NotFound("La Pelicula ingresada para eliminar NO EXISTE.");
            }

           _context.Peliculas.Remove(eliminarPelicula);

            _context.SaveChanges();

            return Ok(_context.Peliculas.ToList());
        }

    }
}
