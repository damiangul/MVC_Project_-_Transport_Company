using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class KlientModel
    {
        public int KlientID { get; set; }
        [DisplayName("Podaj imię")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długie imię")]
        public string KlientImie { get; set; }
        [DisplayName("Podaj nazwisko")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długie nazwisko")]
        public string KlientNazwisko { get; set; }
        [DisplayName("Podaj telefon")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(9, ErrorMessage = "Za długie numer telefonu")]
        public string KlientTelefon { get; set; }
        [DisplayName("Podaj email")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długie numer telefonu")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
        public string KlientEmail { get; set; }
        [DisplayName("Podaj ulicę zamieszkania")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Za długa nazwa ulicy")]
        public string KlientUlica { get; set; }
        [DisplayName("Podaj numer domu")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(6, ErrorMessage = "Za długi numer")]
        public string KlientNumer { get; set; }
        [DisplayName("Podaj miasto")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długa nazwa miasta")]
        public string KlientMiasto { get; set; }
        [DisplayName("Podaj kod")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(6, ErrorMessage = "Za długi kod")]
        public string KlientKod { get; set; }
        [DisplayName("Podaj login")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Za długi login")]
        public string KlientLogin { get; set; }
        [DisplayName("Podaj hasło")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Za długie hasło")]
        public string KlientHaslo { get; set; }
    }
}