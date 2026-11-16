
using System;
using System.Collections.Generic;
using PametnaBiblioteka.Helpers;
using PametnaBiblioteka.Models;
using PametnaBiblioteka.Services;

namespace PametnaBiblioteka.Menus
{
   
    public class KnjigeMeni
    {
        private readonly KnjigaService servis;

        public KnjigeMeni(KnjigaService knjigaService)
        {
            servis = knjigaService;
        }

        public void Pokreni()
        {
            while (true)
            {
                Console.WriteLine("\n===== INVENTAR KNJIGA =====");
                Console.WriteLine("1. Dodaj novu knjigu");
                Console.WriteLine("2. Prikazi sve knjige");
                Console.WriteLine("3. Pretrazi knjige");
                Console.WriteLine("4. Azuriraj knjigu");
                Console.WriteLine("5. Obrisi knjigu");
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
                            Dodavanje();
                            break;
                        case "2":
                            Prikazi(servis.SveKnjige());
                            break;
                        case "3":
                            Pretraga();
                            break;
                        case "4":
                            Azuriranje();
                            break;
                        case "5":
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

        private void Dodavanje()
        {
            Console.WriteLine("\n--- DODAVANJE KNJIGE ---");

            Console.Write("Naslov: ");
            string naslov = Console.ReadLine() ?? "";

            Console.Write("Autor: ");
            string autor = Console.ReadLine() ?? "";

            Console.Write("Zanr: ");
            string zanr = Console.ReadLine() ?? "";

            int ukupno = Konzola.UnesiPozitivanBroj(
                "Ukupan broj primjeraka: ");

            Knjiga knjiga = servis.DodajKnjigu(
                naslov, autor, zanr, ukupno);

            Console.WriteLine(
                $"Knjiga je uspjesno dodana! ID: {knjiga.Id}");
        }

        private void Prikazi(IReadOnlyList<Knjiga> knjige)
        {
            Console.WriteLine("\n--- LISTA KNJIGA ---");

            if (knjige.Count == 0)
            {
                Console.WriteLine("Nema pronadjenih knjiga.");
                return;
            }

            foreach (Knjiga k in knjige)
            {
                int posudjeno =
                    k.UkupnoPrimjeraka - k.DostupnoPrimjeraka;

                Console.WriteLine("---------------------------");
                Console.WriteLine($"ID: {k.Id}");
                Console.WriteLine($"Naslov: {k.Naslov}");
                Console.WriteLine($"Autor: {k.Autor}");
                Console.WriteLine($"Zanr: {k.Zanr}");
                Console.WriteLine(
                    $"Ukupno primjeraka: {k.UkupnoPrimjeraka}");
                Console.WriteLine(
                    $"Dostupno primjeraka: {k.DostupnoPrimjeraka}");
                Console.WriteLine(
                    $"Posudjeno primjeraka: {posudjeno}");
            }

            Console.WriteLine("---------------------------");
        }

        private void Pretraga()
        {
            Console.Write("\nUnesi naslov, autora ili zanr: ");
            string upit = Console.ReadLine() ?? "";

            Prikazi(servis.PretraziKnjige(upit));
        }

        private void Azuriranje()
        {
            Console.WriteLine("\n--- AZURIRANJE KNJIGE ---");

            int id = Konzola.UnesiPozitivanBroj(
                "Unesi ID knjige: ");

            Knjiga? knjiga = servis.NadjiPoId(id);

            if (knjiga == null)
            {
                Console.WriteLine("Knjiga ne postoji.");
                return;
            }

            Console.WriteLine(
                "Pritisni Enter za zadrzavanje postojece vrijednosti.");

            string naslov = Konzola.ProcitajNovuVrijednost(
                "Novi naslov", knjiga.Naslov);

            string autor = Konzola.ProcitajNovuVrijednost(
                "Novi autor", knjiga.Autor);

            string zanr = Konzola.ProcitajNovuVrijednost(
                "Novi zanr", knjiga.Zanr);

            int ukupno = Konzola.ProcitajNoviUkupniBroj(
                knjiga.UkupnoPrimjeraka);

            servis.AzurirajKnjigu(
                id, naslov, autor, zanr, ukupno);

            Console.WriteLine("Knjiga je uspjesno azurirana.");
        }

        private void Brisanje()
        {
            Console.WriteLine("\n--- BRISANJE KNJIGE ---");

            int id = Konzola.UnesiPozitivanBroj(
                "Unesi ID knjige: ");

            Knjiga? knjiga = servis.NadjiPoId(id);

            if (knjiga == null)
            {
                Console.WriteLine("Knjiga ne postoji.");
                return;
            }

            Console.WriteLine($"Naslov: {knjiga.Naslov}");
            Console.WriteLine($"Autor: {knjiga.Autor}");

            if (!Konzola.PotvrdiBrisanje())
            {
                Console.WriteLine("Brisanje otkazano.");
                return;
            }

            servis.ObrisiKnjigu(id);

            Console.WriteLine("Knjiga je uspjesno obrisana.");
        }
    }
}
