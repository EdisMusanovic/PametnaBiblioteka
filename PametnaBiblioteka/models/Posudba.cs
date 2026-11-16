
using System;

namespace PametnaBiblioteka.Models
{
    public class Posudba
    {
        public int Id { get; set; }

        public int KorisnikId { get; set; }

        public int KnjigaId { get; set; }

        public DateTime DatumPosudbe { get; set; }

        public DateTime RokVracanja => DatumPosudbe.AddDays(14);

        public DateTime? DatumVracanja { get; set; }

      
        public bool JeVracena => DatumVracanja.HasValue;
    }
}
