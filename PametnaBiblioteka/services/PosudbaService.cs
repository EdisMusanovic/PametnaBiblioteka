
using System;
using System.Collections.Generic;
using System.Linq;
using PametnaBiblioteka.Data;
using PametnaBiblioteka.Models;

namespace PametnaBiblioteka.Services
{
    public class PosudbaService
    {
        private readonly BibliotekaPodaci podaci;

        private const int RokDana = 14;
        private const decimal KaznaPoDanu = 0.50m;

        public PosudbaService(BibliotekaPodaci podaci)
        {
            this.podaci = podaci;
        }

        public Posudba PosudiKnjigu(int korisnikId, int knjigaId)
        {
            if (korisnikId <= 0 || knjigaId <= 0)
            {
                throw new ArgumentException(
                    "ID korisnika i knjige moraju biti pozitivni.");
            }

            Korisnik? korisnik = podaci.Korisnici
                .FirstOrDefault(k => k.Id == korisnikId);

            if (korisnik == null)
            {
                throw new KeyNotFoundException(
                    "Korisnik s unesenim ID-jem ne postoji.");
            }

            Knjiga? knjiga = podaci.Knjige
                .FirstOrDefault(k => k.Id == knjigaId);

            if (knjiga == null)
            {
                throw new KeyNotFoundException(
                    "Knjiga s unesenim ID-jem ne postoji.");
            }

            if (knjiga.DostupnoPrimjeraka <= 0)
            {
                throw new InvalidOperationException(
                    "Nema dostupnih primjeraka ove knjige.");
            }

            Posudba posudba = new Posudba
            {
                Id = podaci.GenerisiPosudbaId(),
                KorisnikId = korisnikId,
                KnjigaId = knjigaId,
                DatumPosudbe = DateTime.Today,
                DatumVracanja = null
            };

            podaci.Posudbe.Add(posudba);

            knjiga.DostupnoPrimjeraka--;

            return posudba;
        }


        public Posudba? NadjiPoId(int id)
        {
            return podaci.Posudbe
                .FirstOrDefault(p => p.Id == id);
        }

      
        public decimal VratiKnjigu(int posudbaId)
        {
            Posudba? posudba = NadjiPoId(posudbaId);

            if (posudba == null)
            {
                throw new KeyNotFoundException(
                    "Posudba s unesenim ID-jem ne postoji.");
            }

            if (posudba.JeVracena)
            {
                throw new InvalidOperationException(
                    "Ova knjiga je vec vracena.");
            }

            Knjiga? knjiga = podaci.Knjige
                .FirstOrDefault(k => k.Id == posudba.KnjigaId);

            if (knjiga == null)
            {
                throw new InvalidOperationException(
                    "Knjiga povezana s posudbom ne postoji.");
            }

            if (knjiga.DostupnoPrimjeraka >= knjiga.UkupnoPrimjeraka)
            {
                throw new InvalidOperationException(
                    "Nije moguce vratiti knjigu: " +
                    "stanje inventara nije ispravno.");
            }

            posudba.DatumVracanja = DateTime.Today;

            knjiga.DostupnoPrimjeraka++;

            return IzracunajKaznu(posudba);
        }

        public int IzracunajDaneKasnjenja(
            Posudba posudba,
            DateTime? datumProvjere = null)
        {
           
            DateTime datum = posudba.DatumVracanja
                ?? datumProvjere
                ?? DateTime.Today;

            DateTime rok = posudba.DatumPosudbe
                .AddDays(RokDana);

            int brojDana = (datum.Date - rok.Date).Days;

            return Math.Max(0, brojDana);
        }

       
        public decimal IzracunajKaznu(
            Posudba posudba,
            DateTime? datumProvjere = null)
        {
            int dani = IzracunajDaneKasnjenja(
                posudba, datumProvjere);

            return dani * KaznaPoDanu;
        }

       
        public IReadOnlyList<Posudba> AktivnePosudbe()
        {
            return podaci.Posudbe
                .Where(p => !p.JeVracena)
                .OrderBy(p => p.RokVracanja)
                .ThenBy(p => p.Id)
                .ToList();
        }

     
        public IReadOnlyList<Posudba> HistorijaPosudbi()
        {
            return podaci.Posudbe
                .OrderByDescending(p => p.DatumPosudbe)
                .ThenByDescending(p => p.Id)
                .ToList();
        }
    }
}
