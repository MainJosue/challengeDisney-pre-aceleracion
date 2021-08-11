using ChallengeDisney.Context;
using ChallengeDisney.Entidades;
using ChallengeDisney.Model.Genero;
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
    public class GeneroController : ControllerBase
    {
        private readonly Context.ChallengeDisneyContext _context;

        public GeneroController(Context.ChallengeDisneyContext ctx)
        {
            _context = ctx;
        }

        [HttpGet("generos")]   
        public IActionResult GetGenero()
        {
            return Ok(_context.Generos
                .Select(x => new { Nombre = x.Nombre, Imagen = x.Imagen })
                .ToList());
        }

        [HttpGet("detalle")]
        public IActionResult GetGenre(int id)
        {
            var genero = _context.Generos
                .Include(x => x.Peliculas)
                .FirstOrDefault(x => x.Id == id);

            if (genero == null)
            {
                return NotFound("El Genero ingresado NO EXISTE.");
            }

            return Ok(genero);
        }

        [HttpPost("add")]
        public IActionResult Post(GeneroRequestModel genero)
        {
            var nuevoGenero = new Generos
            {
                Nombre = genero.Nombre,
                Imagen = genero.Imagen
            };            

            _context.Generos.Add(nuevoGenero);

            _context.SaveChanges();

            return Ok(new GeneroResponseModel
            {
                Nombre = nuevoGenero.Nombre,
                Imagen = nuevoGenero.Imagen
            });
        }

        [HttpPut("update")]
        public IActionResult Put(Generos genero)
        {
            var nuevoGenero = _context.Generos.FirstOrDefault(x => x.Id == genero.Id);

            if (nuevoGenero == null)
            {
                return NotFound("El Gnero NO EXISTE.");
            }

            nuevoGenero.Imagen = genero.Imagen;
            nuevoGenero.Nombre = genero.Nombre;

            _context.Generos.Update(nuevoGenero);
            _context.SaveChanges();

            return Ok(_context.Generos.ToList());
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Generos genero)
        {
            _context.Generos.Remove(genero);
            _context.SaveChanges();

            return Ok(_context.Generos.ToList());
        }
    }
}
