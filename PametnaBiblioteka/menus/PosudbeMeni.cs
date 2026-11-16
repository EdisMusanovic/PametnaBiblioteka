
using System;
using System.Collections.Generic;
using PametnaBiblioteka.Helpers;
using PametnaBiblioteka.Models;
using PametnaBiblioteka.Services;

namespace PametnaBiblioteka.Menus
{
    public class PosudbeMeni
    {
        private readonly PosudbaService posudbaService;
        private readonly KorisnikService korisnikService;
        private readonly KnjigaService knjigaService;

        public PosudbeMeni(
            PosudbaService posudbaService,
            KorisnikService korisnikService,
            KnjigaService knjigaService)
        {
            this.posudbaService = posudbaService;
            this.korisnikService = korisnikService;
            this.knjigaService = knjigaService;
        }

        public void Pokreni()
        {
            while (true)
            {
                Console.WriteLine("\n===== SISTEM POSUDBE =====");
                Console.WriteLine("1. Posudi knjigu");
                Console.WriteLine("2. Vrati knjigu");
                Console.WriteLine("3. Prikazi aktivne posudbe");
                Console.WriteLine("4. Prikazi historiju posudbi");
                Console.WriteLine("5. Test obracuna kazne");
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
                            PosudbaKnjige();
                            break;
                        case "2":
                            VracanjeKnjige();
                            break;
                        case "3":
                            Console.WriteLine(
                                "\n--- AKTIVNE POSUDBE ---");
                            PrikaziPosudbe(
                                posudbaService.AktivnePosudbe());
                            break;
                        case "4":
                            Console.WriteLine(
                                "\n--- HISTORIJA POSUDBI ---");
                            PrikaziPosudbe(
                                posudbaService.HistorijaPosudbi());
                            break;
                        case "5":
                            TestKazne();
                            break;
                        default:
                            Console.WriteLine("Nepostojeca opcija.");
                            break;
                    }
                });

                Konzola.Pauza();
            }
        }

       
        private void PosudbaKnjige()
        {
            Console.WriteLine("\n--- POSUDBA KNJIGE ---");

            int korisnikId = Konzola.UnesiPozitivanBroj(
                "Unesi ID korisnika: ");

            int knjigaId = Konzola.UnesiPozitivanBroj(
                "Unesi ID knjige: ");

            Posudba posudba = posudbaService.PosudiKnjigu(
                korisnikId, knjigaId);

            Console.WriteLine("\nKnjiga je uspjesno posudjena!");
            Console.WriteLine($"ID posudbe: {posudba.Id}");
            Console.WriteLine(
                $"Datum posudbe: {posudba.DatumPosudbe:dd.MM.yyyy.}");
            Console.WriteLine(
                $"Rok vracanja: {posudba.RokVracanja:dd.MM.yyyy.}");
        }

    
        private void VracanjeKnjige()
        {
            Console.WriteLine("\n--- VRACANJE KNJIGE ---");

            int id = Konzola.UnesiPozitivanBroj(
                "Unesi ID posudbe: ");

            decimal kazna = posudbaService.VratiKnjigu(id);

            Console.WriteLine("Knjiga je uspjesno vracena!");

            if (kazna > 0)
            {
                Console.WriteLine(
                    $"Kazna: {Konzola.FormatirajNovac(kazna)} KM");
            }
            else
            {
                Console.WriteLine("Nema kazne za kasnjenje.");
            }
        }

        
        private void PrikaziPosudbe(IReadOnlyList<Posudba> posudbe)
        {
            if (posudbe.Count == 0)
            {
                Console.WriteLine("Nema evidentiranih posudbi.");
                return;
            }

            foreach (Posudba p in posudbe)
            {
                Korisnik? korisnik = korisnikService.NadjiPoId(
                    p.KorisnikId);

                Knjiga? knjiga = knjigaService.NadjiPoId(
                    p.KnjigaId);

                string imeKorisnika = korisnik != null
                    ? $"{korisnik.Ime} {korisnik.Prezime}"
                    : $"Nepoznat korisnik (ID: {p.KorisnikId})";

                string naslovKnjige = knjiga != null
                    ? knjiga.Naslov
                    : $"Nepoznata knjiga (ID: {p.KnjigaId})";

                int daniKasnjenja = posudbaService
                    .IzracunajDaneKasnjenja(p);

                decimal kazna = posudbaService
                    .IzracunajKaznu(p);

                Console.WriteLine("---------------------------");
                Console.WriteLine($"ID posudbe: {p.Id}");
                Console.WriteLine($"Korisnik: {imeKorisnika}");
                Console.WriteLine($"Knjiga: {naslovKnjige}");
                Console.WriteLine(
                    $"Datum posudbe: {p.DatumPosudbe:dd.MM.yyyy.}");
                Console.WriteLine(
                    $"Rok vracanja: {p.RokVracanja:dd.MM.yyyy.}");

                if (p.JeVracena)
                {
                    Console.WriteLine("Status: Vracena");
                    Console.WriteLine(
                        $"Datum vracanja: {p.DatumVracanja:dd.MM.yyyy.}");
                    Console.WriteLine(
                        $"Dani kasnjenja: {daniKasnjenja}");
                    Console.WriteLine(
                        $"Konacna kazna: " +
                        $"{Konzola.FormatirajNovac(kazna)} KM");
                }
                else
                {
                    Console.WriteLine("Status: Aktivna");
                    Console.WriteLine(
                        $"Trenutni dani kasnjenja: {daniKasnjenja}");
                    Console.WriteLine(
                        $"Trenutna kazna: " +
                        $"{Konzola.FormatirajNovac(kazna)} KM");
                }
            }

            Console.WriteLine("---------------------------");
        }

        
        private void TestKazne()
        {
            Console.WriteLine("\n--- TEST OBRACUNA KAZNE ---");

            Posudba testnaPosudba = new Posudba
            {
                DatumPosudbe = DateTime.Today.AddDays(-20),
                DatumVracanja = DateTime.Today
            };

            int dani = posudbaService
                .IzracunajDaneKasnjenja(testnaPosudba);

            decimal kazna = posudbaService
                .IzracunajKaznu(testnaPosudba);

            Console.WriteLine(
                $"Datum posudbe: {testnaPosudba.DatumPosudbe:dd.MM.yyyy.}");
            Console.WriteLine(
                $"Rok vracanja: {testnaPosudba.RokVracanja:dd.MM.yyyy.}");
            Console.WriteLine(
                $"Datum vracanja: {testnaPosudba.DatumVracanja:dd.MM.yyyy.}");
            Console.WriteLine($"Dani kasnjenja: {dani}");
            Console.WriteLine(
                $"Ukupna kazna: {Konzola.FormatirajNovac(kazna)} KM");

            Console.WriteLine(
                "\nOvo je samo test. Stvarni podaci nisu promijenjeni.");
        }
    }
}
