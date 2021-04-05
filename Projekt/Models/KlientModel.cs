using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class KlientModel
    {
        public int KlientID { get; set; }
        [DisplayName("Podaj imię")]
        public string KlientImie { get; set; }
        [DisplayName("Podaj nazwisko")]
        public string KlientNazwisko { get; set; }
        [DisplayName("Podaj telefon")]
        public string KlientTelefon { get; set; }
        [DisplayName("Podaj email")]
        public string KlientEmail { get; set; }
        [DisplayName("Podaj ulicę zamieszkania")]
        public string KlientUlica { get; set; }
        [DisplayName("Podaj numer domu")]
        public string KlientNumer { get; set; }
        [DisplayName("Podaj miasto")]
        public string KlientMiasto { get; set; }
        [DisplayName("Podaj kod")]
        public string KlientKod { get; set; }
        [DisplayName("Podaj login")]
        public string KlientLogin { get; set; }
        [DisplayName("Podaj hasło")]
        public string KlientHaslo { get; set; }
    }
}