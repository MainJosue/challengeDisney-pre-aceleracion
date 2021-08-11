using ChallengeDisney.Entidades;
using ChallengeDisney.Model.Registrar;
using ChallengeDisney.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace ChallengeDisney.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
      
        private readonly UserManager<Usuario> _usuarioContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AutenticacionController(UserManager<Usuario> usuarioctx, IConfiguration conf, IMailService mailService)
        {
            _usuarioContext = usuarioctx;
            _configuration = conf;
            _mailService = mailService;

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrarRequestModel registro)
        {
            var usuarioExiste = await _usuarioContext.FindByNameAsync(registro.Username);

            if (usuarioExiste != null)
            {
                return BadRequest();
            }

            var usuario = new Usuario
            {
                Email = registro.Email,
                UserName = registro.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var resultado = await _usuarioContext.CreateAsync(usuario, registro.Password);

            if (!resultado.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Status = "Error",
                        Message = $"Creacion de Usuario Fallida! Errores: {string.Join(',', resultado.Errors.Select(x => x.Description))}"
                    });
            }

            await _mailService.SendEmailAsync(usuario);

            return Ok(new
            {
                Status = "Succes",
                Message = "Usuario creado correctamente"
            });

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] InfoUsuarioRequestModel infoUsuario)
        {
            var usuario = await _usuarioContext.FindByNameAsync(infoUsuario.Username);

            if (usuario == null || !await _usuarioContext.CheckPasswordAsync(usuario, infoUsuario.Password))
            {
                return Unauthorized();
            }

            var usuarioRoles = await _usuarioContext.GetRolesAsync(usuario);
            var autenticacionClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var expiracion = DateTime.UtcNow.AddHours(5);
            autenticacionClaims.AddRange(usuarioRoles.Select(usuarioRoles => new Claim(ClaimTypes.Role, usuarioRoles)));

            var autenticacionLlave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LlaveSecreta"]));

            var token = new JwtSecurityToken
                (
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: autenticacionClaims,
                    expires: expiracion,
                    signingCredentials: new SigningCredentials(autenticacionLlave, SecurityAlgorithms.HmacSha256)
                );



            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });        }
        
    } 
}
