
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PametnaBiblioteka.Helpers
{
  
    public static class Konzola
    {
      
        public static int UnesiPozitivanBroj(string poruka)
        {
            Console.Write(poruka);
            string unos = Console.ReadLine() ?? "";

            if (!int.TryParse(unos, out int broj) || broj <= 0)
            {
                throw new ArgumentException(
                    "Morate unijeti pozitivan cijeli broj.");
            }

            return broj;
        }

     
        public static string ProcitajNovuVrijednost(
            string naziv, string postojecaVrijednost)
        {
            Console.Write($"{naziv} [{postojecaVrijednost}]: ");

            string unos = Console.ReadLine() ?? "";

            return string.IsNullOrWhiteSpace(unos)
                ? postojecaVrijednost
                : unos.Trim();
        }

        public static int ProcitajNoviUkupniBroj(int postojeciBroj)
        {
            Console.Write(
                $"Novi ukupni broj primjeraka [{postojeciBroj}]: ");

            string unos = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(unos))
            {
                return postojeciBroj;
            }

            if (!int.TryParse(unos, out int broj) || broj <= 0)
            {
                throw new ArgumentException(
                    "Broj primjeraka mora biti pozitivan cijeli broj.");
            }

            return broj;
        }

       
        public static bool PotvrdiBrisanje()
        {
            Console.Write("Potvrdi brisanje unosom DA: ");

            string unos = Console.ReadLine() ?? "";

            return unos.Equals(
                "DA", StringComparison.OrdinalIgnoreCase);
        }

        public static string FormatirajNovac(decimal iznos)
        {
            return iznos.ToString(
                "0.00", CultureInfo.GetCultureInfo("bs-BA"));
        }

       
        public static void IzvrsiSigurno(Action akcija)
        {
            try
            {
                akcija();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }

        public static void Pauza()
        {
            Console.WriteLine(
                "\nPritisni Enter za nastavak...");
            Console.ReadLine();
        }
    }
}
