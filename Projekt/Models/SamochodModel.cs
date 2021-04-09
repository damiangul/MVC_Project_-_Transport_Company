using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
    public class SamochodModel
    {
        public int SamochodID { get; set; }

        [DisplayName("Podaj nazwe Marki")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długa nazwa")]
        public string SamochodMarka { get; set; }

        [DisplayName("Podaj model")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długa nazwa")]
        public string SamochodModelPojazdu { get; set; }

        [DisplayName("Podaj date produkcji")]
        [Required]
        public DateTime SamochodDataProdukcji { get; set; }

        [DisplayName("Podaj pojemnosc w litrach pojazdu")]
        [Required]
        public int SamochodPojemnosc { get; set; }
        [DisplayName("Podaj przebieg pojazdu")]
        [Required]
        public int SamochodPrzebieg { get; set; }
        [DisplayName("Podaj datę ważności przeglądu")]
        [Required]
        public DateTime SamochodPrzeglad { get; set; }

    }
}