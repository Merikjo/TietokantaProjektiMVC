//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TietokantaProjektiMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tunnit
    {
        public int TuntiID { get; set; }
        public Nullable<int> ProjektiID { get; set; }
        public Nullable<int> HenkiloID { get; set; }
        public Nullable<System.DateTime> Pvm { get; set; }
        public Nullable<int> ProjektiTunti { get; set; }
        public Nullable<decimal> ProjektiTunnit { get; set; }
        public Nullable<decimal> SuunnitellutTunnit { get; set; }
        public Nullable<decimal> ToteutuneetTunnit { get; set; }
    
        public virtual Henkilot Henkilot { get; set; }
        public virtual Projektit Projektit { get; set; }
    }
}
