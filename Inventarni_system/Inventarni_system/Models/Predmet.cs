namespace Inventarni_system.Models
{
    public class Predmet
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public int SkladId { get; set; }
        public int Mnozstvi { get; set; }
        public decimal CenaZaKus { get; set; }
        public Sklad Sklad { get; set; }

    }
}
