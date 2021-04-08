using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class DodanieStatusuPracownik
    {
        public List<StatusPaczki> Statusy { get; set; }
        public int SelectedStatus { get; set; }
        public DateTime ZmianaStanuPrzesylki { get; set; }
        public string Uwagi { get; set; }

        public int PrzesylkaID { get; set; }
    }
}