
using System;
using System.Collections.Generic;
using System.Linq;
using PametnaBiblioteka.Data;
using PametnaBiblioteka.Helpers;
using PametnaBiblioteka.Models;

namespace PametnaBiblioteka.Services
{
    public class KorisnikService
    {
        private readonly BibliotekaPodaci podaci;

        public KorisnikService(BibliotekaPodaci podaci)
        {
            this.podaci = podaci;
        }

        public Korisnik RegistrujKorisnika(
            string ime, string prezime, string email)
        {
            Validacija.ProvjeriIme(ime, "Ime");
            Validacija.ProvjeriIme(prezime, "Prezime");
            Validacija.ProvjeriEmail(email);

            ime = ime.Trim();
            prezime = prezime.Trim();
            email = email.Trim();

            if (EmailPostoji(email))
            {
                throw new InvalidOperationException(
                    "Korisnik s ovom email adresom vec postoji.");
            }

            Korisnik korisnik = new Korisnik
            {
                Id = podaci.GenerisiKorisnikId(),
                Ime = ime,
                Prezime = prezime,
                Email = email,
                UlogaKorisnika = Uloga.Clan
            };

            podaci.Korisnici.Add(korisnik);

            return korisnik;
        }

        public IReadOnlyList<Korisnik> SviKorisnici()
        {
            return podaci.Korisnici
                .OrderBy(k => k.Id)
                .ToList();
        }

        public Korisnik? NadjiPoId(int id)
        {
            return podaci.Korisnici
                .FirstOrDefault(k => k.Id == id);
        }

        public IReadOnlyList<Korisnik> PretraziKorisnike(
            string upit)
        {
            if (string.IsNullOrWhiteSpace(upit))
            {
                return SviKorisnici();
            }

            string tekst = upit.Trim();

            return podaci.Korisnici
                .Where(k =>
                    k.Ime.Contains(
                        tekst, StringComparison.OrdinalIgnoreCase) ||
                    k.Prezime.Contains(
                        tekst, StringComparison.OrdinalIgnoreCase) ||
                    k.Email.Contains(
                        tekst, StringComparison.OrdinalIgnoreCase) ||
                    (k.Ime + " " + k.Prezime).Contains(
                        tekst, StringComparison.OrdinalIgnoreCase))
                .OrderBy(k => k.Id)
                .ToList();
        }

        public void AzurirajKorisnika(
            int id, string ime, string prezime, string email)
        {
            Korisnik korisnik = PronadjiObaveznogKorisnika(id);

            Validacija.ProvjeriIme(ime, "Ime");
            Validacija.ProvjeriIme(prezime, "Prezime");
            Validacija.ProvjeriEmail(email);

            ime = ime.Trim();
            prezime = prezime.Trim();
            email = email.Trim();

            if (EmailPostoji(email, id))
            {
                throw new InvalidOperationException(
                    "Email adresu vec koristi drugi korisnik.");
            }

            korisnik.Ime = ime;
            korisnik.Prezime = prezime;
            korisnik.Email = email;
        }

        public void PromijeniUlogu(int id, Uloga novaUloga)
        {
            Korisnik korisnik = PronadjiObaveznogKorisnika(id);

            if (!Enum.IsDefined(typeof(Uloga), novaUloga))
            {
                throw new ArgumentException(
                    "Nepostojeca korisnicka uloga.");
            }

            korisnik.UlogaKorisnika = novaUloga;
        }

        public void ObrisiKorisnika(int id)
        {
            Korisnik korisnik = PronadjiObaveznogKorisnika(id);

            bool imaPosudbe = podaci.Posudbe
                .Any(p => p.KorisnikId == id);

            if (imaPosudbe)
            {
                throw new InvalidOperationException(
                    "Nije moguce obrisati korisnika " +
                    "koji ima historiju posudbi.");
            }

            podaci.Korisnici.Remove(korisnik);
        }

        private bool EmailPostoji(
            string email, int? izuzmiId = null)
        {
            return podaci.Korisnici.Any(k =>
                k.Email.Equals(
                    email, StringComparison.OrdinalIgnoreCase) &&
                (!izuzmiId.HasValue || k.Id != izuzmiId.Value));
        }

        private Korisnik PronadjiObaveznogKorisnika(int id)
        {
            return NadjiPoId(id)
                ?? throw new KeyNotFoundException(
                    "Korisnik s unesenim ID-jem ne postoji.");
        }
    }
}
