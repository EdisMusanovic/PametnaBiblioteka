
using System;
using System.Linq;
using System.Net.Mail;

namespace PametnaBiblioteka.Helpers
{
    public static class Validacija
    {
        public static void ProvjeriIme(
            string vrijednost, string nazivPolja)
        {
            if (string.IsNullOrWhiteSpace(vrijednost))
            {
                throw new ArgumentException(
                    $"{nazivPolja} ne smije biti prazno.");
            }

            string tekst = vrijednost.Trim();

            if (tekst.Length < 2 || tekst.Length > 50)
            {
                throw new ArgumentException(
                    $"{nazivPolja} mora imati 2 do 50 znakova.");
            }

            bool ispravno = tekst.All(c =>
                char.IsLetter(c) ||
                c == ' ' ||
                c == '-' ||
                c == '\'');

            if (!ispravno)
            {
                throw new ArgumentException(
                    $"{nazivPolja} sadrzi nedozvoljene znakove.");
            }
        }

        public static void ProvjeriEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(
                    "Email ne smije biti prazan.");
            }

            string tekst = email.Trim();

            bool ispravanFormat =
                tekst.Length <= 100 &&
                MailAddress.TryCreate(tekst, out MailAddress? adresa) &&
                adresa != null &&
                adresa.Address.Equals(
                    tekst, StringComparison.OrdinalIgnoreCase) &&
                adresa.Host.Contains('.');

            if (!ispravanFormat)
            {
                throw new ArgumentException(
                    "Email adresa nije ispravna.");
            }
        }

        public static void ProvjeriTekst(
            string vrijednost,
            string nazivPolja,
            int maksimalnaDuzina)
        {
            if (string.IsNullOrWhiteSpace(vrijednost))
            {
                throw new ArgumentException(
                    $"{nazivPolja} ne smije biti prazno.");
            }

            if (vrijednost.Trim().Length > maksimalnaDuzina)
            {
                throw new ArgumentException(
                    $"{nazivPolja} ne smije imati vise od " +
                    $"{maksimalnaDuzina} znakova.");
            }
        }
    }
}
