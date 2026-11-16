
using System.Collections.Generic;
using PametnaBiblioteka.Models;

namespace PametnaBiblioteka.Data
{
    public class BibliotekaPodaci
    {
        public List<Korisnik> Korisnici { get; } = new();

        public List<Knjiga> Knjige { get; } = new();

        public List<Posudba> Posudbe { get; } = new();

        private int sljedeciKorisnikId = 1;
        private int sljedeciKnjigaId = 1;
        private int sljedeciPosudbaId = 1;

        public int GenerisiKorisnikId()
        {
            return sljedeciKorisnikId++;
        }

        public int GenerisiKnjigaId()
        {
            return sljedeciKnjigaId++;
        }

        public int GenerisiPosudbaId()
        {
            return sljedeciPosudbaId++;
        }
    }
}
