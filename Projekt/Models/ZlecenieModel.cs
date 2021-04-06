using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class ZlecenieModel
    {
        [DisplayName("Podaj opis")]
        public string ZlecenieOpis { get; set; }
        [DisplayName("Podaj wagę")]
        public string ZlecenieWaga { get; set; }
        [DisplayName("Podaj ilość")]
        public string ZlecenieIlosc { get; set; }
    }
}