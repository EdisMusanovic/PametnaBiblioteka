
using PametnaBiblioteka.Services;

namespace PametnaBiblioteka.Data
{
    
    public static class PocetniPodaci
    {
        public static void Ucitaj(
            KorisnikService korisnikService,
            KnjigaService knjigaService)
        {
         

            korisnikService.RegistrujKorisnika(
                "Amar",
                "Hodžić",
                "amar@gmail.com"
            );

            korisnikService.RegistrujKorisnika(
                "Lejla",
                "Kovačević",
                "lejla@gmail.com"
            );

            korisnikService.RegistrujKorisnika(
                "Emir",
                "Hadžić",
                "emir@gmail.com"
            );


            knjigaService.DodajKnjigu(
                "Na Drini ćuprija",
                "Ivo Andrić",
                "Roman",
                5
            );

            knjigaService.DodajKnjigu(
                "Prokleta avlija",
                "Ivo Andrić",
                "Roman",
                3
            );

            knjigaService.DodajKnjigu(
                "Derviš i smrt",
                "Meša Selimović",
                "Roman",
                4
            );
        }
    }
}
