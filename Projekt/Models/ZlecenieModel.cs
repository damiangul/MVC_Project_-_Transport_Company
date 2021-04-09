using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class ZlecenieModel
    {
        [Required(AllowEmptyStrings = false)]
        [DisplayName("Podaj opis")]
        [StringLength(255, ErrorMessage = "Za długi opis")]
        public string ZlecenieOpis { get; set; }

        [Required]
        [DisplayName("Podaj wagę")]
        public string ZlecenieWaga { get; set; }
        [Required]
        [DisplayName("Podaj ilość")]
        public string ZlecenieIlosc { get; set; }
    }
}