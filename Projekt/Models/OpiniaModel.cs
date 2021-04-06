using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class OpiniaModel
    {
        public int KlientID { get; set; }
        public int PracownikID { get; set; }
        [DisplayName("Podaj opinie")]
        public string Opis { get; set; }
    }
}