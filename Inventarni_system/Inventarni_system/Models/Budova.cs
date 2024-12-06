using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Inventarni_system.Models
{
    public class Budova
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Název budovy je povinný.")]
        [Display(Name = "Název budovy")]
        public string Nazev { get; set; }

        [Required(ErrorMessage = "Typ budovy je povinný.")]
        [Display(Name = "Typ budovy")]
        public string Typ { get; set; } // Např. "Servis" nebo "Obchod"

        [ValidateNever]
        public ICollection<Sklad> Sklady { get; set; } = new List<Sklad>();
    }
}
