//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkhausMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Zahlung
    {
        public int ZahlungID { get; set; }
        public decimal Preis { get; set; }
        public int DauermieterID { get; set; }
        public System.DateTime Datum { get; set; }
    
        public virtual Dauermieter Dauermieter { get; set; }
    }
}
