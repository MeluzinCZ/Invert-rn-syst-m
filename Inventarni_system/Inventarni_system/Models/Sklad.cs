using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Inventarni_system.Models
{
    public class Sklad
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Název skladu je povinný.")]
        [Display(Name = "Název skladu")]
        public string Nazev { get; set; }

        [Required(ErrorMessage = "Budova je povinná.")]
        [Display(Name = "Budova")]
        public int BudovaId { get; set; }

        [ForeignKey("BudovaId")]
        [ValidateNever] // Zabránení validace navigační vlastnosti
        public Budova Budova { get; set; }
    }
}
