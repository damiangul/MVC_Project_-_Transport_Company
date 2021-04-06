using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class DodajZlecenieModel
    {
        [DisplayName("Podaj ID pracownika")]
        public int PracownikID { get; set; }
        [DisplayName("Podaj ID  klienta")]
        public int KlientID { get; set; }
        [DisplayName("Podaj ID samochodu")]
        public int SamochodID { get; set; }
        [DisplayName("Podaj datę przyjęcia zamówienia")]
        public DateTime DataPrzewozu { get; set; }
        [DisplayName("Podaj cenę za kilometr")]
        public Decimal CennaZaKm { get; set; }
        [DisplayName("Podaj długość trasy")]
        public int DlugoscTrasy { get; set; }
        [DisplayName("Podaj opis przesyłki")]
        public string ZlecenieOpis { get; set; }
        [DisplayName("Podaj wagę przesyłki")]
        public int ZlecenieWaga { get; set; }
        [DisplayName("Podaj ilość")]
        public int ZlecenieIlosc { get; set; }
    }
}