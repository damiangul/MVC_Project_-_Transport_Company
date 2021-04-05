using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Projekt.Models
{
    public class SamochodModel
    {
        public int SamochodID { get; set; }
        [DisplayName("Podaj nazwe Marki")]
        public string SamochodMarka { get; set; }
        [DisplayName("Podaj model")]
        public string SamochodModelPojazdu { get; set; }
        [DisplayName("Podaj date produkcji")]
        public DateTime SamochodDataProdukcji { get; set; }
        [DisplayName("Podaj pojemnosc w litrach pojazdu")]
        public int SamochodPojemnosc { get; set; }
        [DisplayName("Podaj przebieg pojazdu")]
        public int SamochodPrzebieg { get; set; }
        [DisplayName("Podaj datę ważności przeglądu")]
        public DateTime SamochodPrzeglad { get; set; }

    }
}