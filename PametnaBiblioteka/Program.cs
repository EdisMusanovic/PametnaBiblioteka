
using System;
using System.Text;
using PametnaBiblioteka.Data;
using PametnaBiblioteka.Menus;
using PametnaBiblioteka.Services;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

BibliotekaPodaci podaci = new BibliotekaPodaci();

KorisnikService korisnikService = new KorisnikService(podaci);
KnjigaService knjigaService = new KnjigaService(podaci);
PosudbaService posudbaService = new PosudbaService(podaci);

PocetniPodaci.Ucitaj(korisnikService, knjigaService);

KorisniciMeni korisniciMeni = new KorisniciMeni(
    korisnikService);

KnjigeMeni knjigeMeni = new KnjigeMeni(
    knjigaService);

PosudbeMeni posudbeMeni = new PosudbeMeni(
    posudbaService, korisnikService, knjigaService);

bool radi = true;

while (radi)
{
    Console.WriteLine("\n====================================");
    Console.WriteLine("       PAMETNA BIBLIOTEKA");
    Console.WriteLine("====================================");

    Console.WriteLine("\n        GLAVNI MENI");
    Console.WriteLine("\n1. Upravljanje korisnicima");
    Console.WriteLine("2. Inventar knjiga");
    Console.WriteLine("3. Sistem posudbe");
    Console.WriteLine("\n0. Izlaz iz aplikacije");

    Console.Write("\nIzaberite opciju: ");

    string opcija = Console.ReadLine()?.Trim() ?? "";

    switch (opcija)
    {
        case "1":
            korisniciMeni.Pokreni();
            break;

        case "2":
            knjigeMeni.Pokreni();
            break;

        case "3":
            posudbeMeni.Pokreni();
            break;

        case "0":
            radi = false;
            Console.WriteLine(
                "\nHvala sto koristite PametnuBiblioteku!");
            break;

        default:
            Console.WriteLine("Nepostojeca opcija.");
            break;
    }
}
