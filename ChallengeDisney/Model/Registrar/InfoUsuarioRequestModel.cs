using System;
using System.ComponentModel.DataAnnotations;

namespace ChallengeDisney.Model.Registrar
{
    public class InfoUsuarioRequestModel 
    {

        [Required]       
        public string Username { get; set; }
       
        [Required]        
        public string Password { get; set; }
    }
}