using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TietokantaProjektiMVC.Models;

namespace TietokantaProjektiMVC.Models
{
    public class SimplyProjektitData
    {
        public SimplyProjektitData()
        {
            this.SimplyTunnitData = new HashSet<SimplyTunnitData>();
            this.SimplyHenkilotData = new HashSet<SimplyHenkilotData>();
        }


        public int? TuntiID { get; set; }
        public int? HenkiloID { get; set; }
        public int ProjektiID { get; set; }
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Osoite { get; set; }
        public string Postinumero { get; set; }
        public int? Esimies { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:HH\\:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Pvm { get; set; }
        //public string Pvm { get; set; }

        public DateTime? Avattu { get; set; }
        public DateTime? Suljettu { get; set; }

        public int? ProjektiTunti { get; set; }
        public decimal? ProjektiTunnit { get; set; }
        public decimal? SuunnitellutTunnit { get; set; }
        public decimal? ToteutuneetTunnit { get; set; }
        public string ProjektiNimi { get; set; }

        public int? Status2 { get; set; }

        //public int? Status
        //{
        //    get { return (ProjektiTunnit / SuunnitellutTunnit) * 100; }
        //    set { Status2 = value; }
        //}

        public string Status { get; set; }

        [Display(Name = "Henkilön nimi")]
        public string KokonimiH2 { get; set; }

        [Display(Name = "Henkilön nimi")]
        public string KokonimiH
        {
            get { return Etunimi + " " + Sukunimi; }
            set { KokonimiH2 = value; }
        }

        public virtual Tunnit Tunnit { get; set; }

        public virtual ICollection<SimplyTunnitData> SimplyTunnitData { get; set; }
        public virtual ICollection<SimplyHenkilotData> SimplyHenkilotData { get; set; }
    }
}