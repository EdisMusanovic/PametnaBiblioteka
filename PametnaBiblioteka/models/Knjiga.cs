
namespace PametnaBiblioteka.Models
{
    public class Knjiga
    {
        public int Id { get; set; }

        public string Naslov { get; set; } = string.Empty;

        public string Autor { get; set; } = string.Empty;

        public string Zanr { get; set; } = string.Empty;

        public int UkupnoPrimjeraka { get; set; }

        public int DostupnoPrimjeraka { get; set; }
    }
}
