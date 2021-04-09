using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class PracownikModel
    {
        public int PracownikID { get; set; }
        [DisplayName("Podaj imię")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(20, ErrorMessage = "Za długie imię")]
        public string PracownikImie { get; set; }
        [DisplayName("Podaj nazwisko")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(25, ErrorMessage = "Za długie nazwisko")]
        public string PracownikNazwisko { get; set; }
        [DisplayName("Podaj telefon")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(9, ErrorMessage = "Za długie numer telefonu")]
        public string PracownikTelefon { get; set; }
        [DisplayName("Podaj email")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długie numer telefonu")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
        public string PracownikEmail { get; set; }
        [DisplayName("Podaj datę zatrudnienia")]
        [Required]
        public DateTime PracownikDataZatrudnienia { get; set; }
        [DisplayName("Podaj pesel")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(11, ErrorMessage = "Za długi numer pesel")]
        public string PracownikPesel { get; set; }
        [DisplayName("Podaj pensje")]
        [Required]
        [Range(2000, 3000, ErrorMessage = "Pensja w przedziale 2000-3000 zł")]
        public Decimal PracownikPensja { get; set; }
        [DisplayName("Podaj ulicę zamieszkania")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Za długa nazwa ulicy")]
        public string PracownikUlica { get; set; }
        [DisplayName("Podaj numer domu")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(6, ErrorMessage = "Za długi numer")]
        public string PracownikNumer { get; set; }
        [DisplayName("Podaj miasto")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(30, ErrorMessage = "Za długa nazwa miasta")]
        public string PracownikMiasto { get; set; }
        [DisplayName("Podaj kod")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(11, ErrorMessage = "Za długi kod")]
        public string PracownikKod { get; set; }
        [DisplayName("Podaj login")]
        [Required(AllowEmptyStrings = false)]
        public string PracownikLogin { get; set; }
        [DisplayName("Podaj hasło")]
        [Required(AllowEmptyStrings = false)]
        public string PracownikHaslo { get; set; }
    }
}