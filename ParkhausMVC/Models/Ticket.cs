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
    
    public partial class Ticket
    {
        public int TicketID { get; set; }
        public System.DateTime Eingangsdatum { get; set; }
        public Nullable<System.DateTime> Ausgangsdatum { get; set; }
        public Nullable<int> ParkplatzID { get; set; }
        public Nullable<decimal> Preis { get; set; }
    
        public virtual Parkplatz Parkplatz { get; set; }
    }
}