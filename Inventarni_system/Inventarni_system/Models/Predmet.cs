using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Inventarni_system.Models
{
    public class Predmet
    {
        public int Id { get; set; }
        [Required]
        public string Nazev { get; set; }
        [Required]
        public int Mnozstvi { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal CenaZaKus { get; set; }
        [ForeignKey("Sklad")]
        public int SkladId { get; set; }
        [ValidateNever]
        public Sklad Sklad { get; set; }

    }
}
