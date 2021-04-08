using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class DodanieStatusuPracownik
    {
        [DisplayName("Wybierz status paczki")]
        public List<StatusPaczki> Statusy { get; set; }
        public int SelectedStatus { get; set; }
        [DisplayName("Wybierz date zmiany statusu")]
        public DateTime ZmianaStanuPrzesylki { get; set; }
        [DisplayName("Dopisz uwagi")]
        public string Uwagi { get; set; }

        public int PrzesylkaID { get; set; }
    }
}