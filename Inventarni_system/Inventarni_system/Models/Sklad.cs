namespace Inventarni_system.Models
{
    public class Sklad
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public int BudovaId { get; set; }
        public Budova Budova { get; set; }
    }
}
