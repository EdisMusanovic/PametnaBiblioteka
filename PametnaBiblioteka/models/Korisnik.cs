
namespace PametnaBiblioteka.Models
{
    public class Korisnik
    {
        public int Id { get; set; }

        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Uloga UlogaKorisnika { get; set; } = Uloga.Clan;
    }
}
