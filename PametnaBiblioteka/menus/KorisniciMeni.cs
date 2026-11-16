
using System;
using System.Collections.Generic;
using PametnaBiblioteka.Helpers;
using PametnaBiblioteka.Models;
using PametnaBiblioteka.Services;

namespace PametnaBiblioteka.Menus
{
  
    public class KorisniciMeni
    {
        private readonly KorisnikService servis;

        public KorisniciMeni(KorisnikService korisnikService)
        {
            servis = korisnikService;
        }

       
        public void Pokreni()
        {
            while (true)
            {
                Console.WriteLine("\n===== UPRAVLJANJE KORISNICIMA =====");
                Console.WriteLine("1. Registruj korisnika");
                Console.WriteLine("2. Prikazi sve korisnike");
                Console.WriteLine("3. Pretrazi korisnike");
                Console.WriteLine("4. Azuriraj korisnika");
                Console.WriteLine("5. Promijeni ulogu");
                Console.WriteLine("6. Obrisi korisnika");
                Console.WriteLine("0. Povratak na glavni meni");
                Console.Write("\nIzaberite opciju: ");

                string opcija = Console.ReadLine()?.Trim() ?? "";

                if (opcija == "0")
                    return;

                Konzola.IzvrsiSigurno(() =>
                {
                    switch (opcija)
                    {
                        case "1":
                            Registracija();
                            break;
                        case "2":
                            Prikazi(servis.SviKorisnici());
                            break;
                        case "3":
                            Pretraga();
                            break;
                        case "4":
                            Azuriranje();
                            break;
                        case "5":
                            PromjenaUloge();
                            break;
                        case "6":
                            Brisanje();
                            break;
                        default:
                            Console.WriteLine("Nepostojeca opcija.");
                            break;
                    }
                });

                Konzola.Pauza();
            }
        }

        private void Registracija()
        {
            Console.WriteLine("\n--- REGISTRACIJA ---");

            Console.Write("Ime: ");
            string ime = Console.ReadLine() ?? "";

            Console.Write("Prezime: ");
            string prezime = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Korisnik korisnik = servis.RegistrujKorisnika(
                ime, prezime, email);

            Console.WriteLine(
                $"Uspjesno registrovan korisnik! ID: {korisnik.Id}");
        }

        private void Prikazi(IReadOnlyList<Korisnik> korisnici)
        {
            Console.WriteLine("\n--- LISTA KORISNIKA ---");

            if (korisnici.Count == 0)
            {
                Console.WriteLine("Nema pronadjenih korisnika.");
                return;
            }

            foreach (Korisnik k in korisnici)
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine($"ID: {k.Id}");
                Console.WriteLine(
                    $"Ime i prezime: {k.Ime} {k.Prezime}");
                Console.WriteLine($"Email: {k.Email}");
                Console.WriteLine($"Uloga: {k.UlogaKorisnika}");
            }

            Console.WriteLine("---------------------------");
        }

        private void Pretraga()
        {
            Console.Write("\nUnesi ime, prezime ili email: ");
            string upit = Console.ReadLine() ?? "";

            Prikazi(servis.PretraziKorisnike(upit));
        }

        private void Azuriranje()
        {
            Console.WriteLine("\n--- AZURIRANJE KORISNIKA ---");

            int id = Konzola.UnesiPozitivanBroj(
                "Unesi ID korisnika: ");

            Korisnik? korisnik = servis.NadjiPoId(id);

            if (korisnik == null)
            {
                Console.WriteLine("Korisnik ne postoji.");
                return;
            }

            Console.WriteLine(
                "Pritisni Enter za zadrzavanje postojece vrijednosti.");

            string ime = Konzola.ProcitajNovuVrijednost(
                "Novo ime", korisnik.Ime);

            string prezime = Konzola.ProcitajNovuVrijednost(
                "Novo prezime", korisnik.Prezime);

            string email = Konzola.ProcitajNovuVrijednost(
                "Novi email", korisnik.Email);

            servis.AzurirajKorisnika(
                id, ime, prezime, email);

            Console.WriteLine("Podaci su uspjesno azurirani.");
        }

        private void PromjenaUloge()
        {
            Console.WriteLine("\n--- PROMJENA ULOGE ---");

            int id = Konzola.UnesiPozitivanBroj(
                "Unesi ID korisnika: ");

            Console.WriteLine("1. Clan");
            Console.WriteLine("2. Administrator");
            Console.Write("Nova uloga: ");

            string izbor = Console.ReadLine() ?? "";

            Uloga uloga;

            if (izbor == "1")
                uloga = Uloga.Clan;
            else if (izbor == "2")
                uloga = Uloga.Administrator;
            else
            {
                Console.WriteLine("Nepostojeca uloga.");
                return;
            }

            servis.PromijeniUlogu(id, uloga);

            Console.WriteLine("Uloga je uspjesno promijenjena.");
        }

        private void Brisanje()
        {
            Console.WriteLine("\n--- BRISANJE KORISNIKA ---");

            int id = Konzola.UnesiPozitivanBroj(
                "Unesi ID korisnika: ");

            Korisnik? korisnik = servis.NadjiPoId(id);

            if (korisnik == null)
            {
                Console.WriteLine("Korisnik ne postoji.");
                return;
            }

            Console.WriteLine(
                $"Korisnik: {korisnik.Ime} {korisnik.Prezime}");

            if (!Konzola.PotvrdiBrisanje())
            {
                Console.WriteLine("Brisanje otkazano.");
                return;
            }

            servis.ObrisiKorisnika(id);

            Console.WriteLine("Korisnik je uspjesno obrisan.");
        }
    }
}
