//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projekt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class KONTO
    {
        public int id { get; set; }
        [DisplayName("Login")]
        public string login_user { get; set; }
        [DisplayName("Has�o")]
        public string password_user { get; set; }
        public string rola { get; set; }
    }
}