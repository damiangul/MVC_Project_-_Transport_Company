using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class OpiniaModel
    {
        public int KlientID { get; set; }
        public int PracownikID { get; set; }
        [DisplayName("Podaj opinie")]
        [StringLength(255, ErrorMessage = "Za długia opinia")]
        public string Opis { get; set; }
    }
}