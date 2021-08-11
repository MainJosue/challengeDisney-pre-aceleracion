using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Model.Registrar
{
    public class RegistrarRequestModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [RegularExpression(@"(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$")]
        public string Password { get; set; }

    }
}
