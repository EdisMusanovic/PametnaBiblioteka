
using System;
using System.Collections.Generic;
using System.Linq;
using PametnaBiblioteka.Data;
using PametnaBiblioteka.Helpers;
using PametnaBiblioteka.Models;

namespace PametnaBiblioteka.Services
{
    public class KnjigaService
    {
        private readonly BibliotekaPodaci podaci;

        public KnjigaService(BibliotekaPodaci podaci)
        {
            this.podaci = podaci;
        }

        public Knjiga DodajKnjigu(
            string naslov,
            string autor,
            string zanr,
            int ukupnoPrimjeraka)
        {
            ProvjeriPodatkeKnjige(
                naslov, autor, zanr, ukupnoPrimjeraka);

            Knjiga knjiga = new Knjiga
            {
                Id = podaci.GenerisiKnjigaId(),
                Naslov = naslov.Trim(),
                Autor = autor.Trim(),
                Zanr = zanr.Trim(),
                UkupnoPrimjeraka = ukupnoPrimjeraka,
                DostupnoPrimjeraka = ukupnoPrimjeraka
            };

            podaci.Knjige.Add(knjiga);

            return knjiga;
        }

        public IReadOnlyList<Knjiga> SveKnjige()
        {
            return podaci.Knjige
                .OrderBy(k => k.Id)
                .ToList();
        }

        public Knjiga? NadjiPoId(int id)
        {
            return podaci.Knjige
                .FirstOrDefault(k => k.Id == id);
        }

        public IReadOnlyList<Knjiga> PretraziKnjige(string upit)
        {
            if (string.IsNullOrWhiteSpace(upit))
            {
                return SveKnjige();
            }

            string tekst = upit.Trim();

            return podaci.Knjige
                .Where(k =>
                    k.Naslov.Contains(
                        tekst, StringComparison.OrdinalIgnoreCase) ||
                    k.Autor.Contains(
                        tekst, StringComparison.OrdinalIgnoreCase) ||
                    k.Zanr.Contains(
                        tekst, StringComparison.OrdinalIgnoreCase))
                .OrderBy(k => k.Id)
                .ToList();
        }

        public void AzurirajKnjigu(
            int id,
            string naslov,
            string autor,
            string zanr,
            int ukupnoPrimjeraka)
        {
            Knjiga knjiga = PronadjiObaveznuKnjigu(id);

            ProvjeriPodatkeKnjige(
                naslov, autor, zanr, ukupnoPrimjeraka);

            int brojPosudjenih =
                knjiga.UkupnoPrimjeraka -
                knjiga.DostupnoPrimjeraka;

            if (ukupnoPrimjeraka < brojPosudjenih)
            {
                throw new InvalidOperationException(
                    "Ukupan broj primjeraka ne moze biti manji " +
                    "od broja trenutno posudjenih primjeraka.");
            }

            knjiga.Naslov = naslov.Trim();
            knjiga.Autor = autor.Trim();
            knjiga.Zanr = zanr.Trim();

            knjiga.UkupnoPrimjeraka = ukupnoPrimjeraka;

            knjiga.DostupnoPrimjeraka =
                ukupnoPrimjeraka - brojPosudjenih;
        }

        public void ObrisiKnjigu(int id)
        {
            Knjiga knjiga = PronadjiObaveznuKnjigu(id);

            bool imaPosudbe = podaci.Posudbe
                .Any(p => p.KnjigaId == id);

            if (imaPosudbe)
            {
                throw new InvalidOperationException(
                    "Knjiga ima historiju posudbi " +
                    "i ne moze biti obrisana.");
            }

            podaci.Knjige.Remove(knjiga);
        }

        private static void ProvjeriPodatkeKnjige(
            string naslov,
            string autor,
            string zanr,
            int ukupnoPrimjeraka)
        {
            Validacija.ProvjeriTekst(
                naslov, "Naslov", 120);

            Validacija.ProvjeriTekst(
                autor, "Autor", 80);

            Validacija.ProvjeriTekst(
                zanr, "Zanr", 50);

            if (ukupnoPrimjeraka <= 0)
            {
                throw new ArgumentException(
                    "Ukupan broj primjeraka mora biti " +
                    "pozitivan cijeli broj.");
            }
        }

        private Knjiga PronadjiObaveznuKnjigu(int id)
        {
            return NadjiPoId(id)
                ?? throw new KeyNotFoundException(
                    "Knjiga s unesenim ID-jem ne postoji.");
        }
    }
}
