using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class PracownikModel
    {
        public int PracownikID { get; set; }
        [DisplayName("Podaj imię")]
        public string PracownikImie { get; set; }
        [DisplayName("Podaj nazwisko")]
        public string PracownikNazwisko { get; set; }
        [DisplayName("Podaj telefon")]
        public string PracownikTelefon { get; set; }
        [DisplayName("Podaj email")]
        public string PracownikEmail { get; set; }
        [DisplayName("Podaj datę zatrudnienia")]
        public DateTime PracownikDataZatrudnienia { get; set; }
        [DisplayName("Podaj pesel")]
        public string PracownikPesel { get; set; }
        [DisplayName("Podaj pensje")]
        public Decimal PracownikPensja { get; set; }
        [DisplayName("Podaj ulicę zamieszkania")]
        public string PracownikUlica { get; set; }
        [DisplayName("Podaj numer domu")]
        public string PracownikNumer { get; set; }
        [DisplayName("Podaj miasto")]
        public string PracownikMiasto { get; set; }
        [DisplayName("Podaj kod")]
        public string PracownikKod { get; set; }
        [DisplayName("Podaj login")]
        public string PracownikLogin { get; set; }
        [DisplayName("Podaj hasło")]
        public string PracownikHaslo { get; set; }
    }
}