using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TietokantaProjektiMVC.Models
{
    public class SimplyTunnitData
    {
        public SimplyTunnitData()
        {
            this.SimplyProjektitData = new HashSet<SimplyProjektitData>();
            this.SimplyHenkilotData = new HashSet<SimplyHenkilotData>();
        }
        public int TuntiID { get; set; }
        public int? HenkiloID { get; set; }
        public int? ProjektiID { get; set; }
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Henkilonimi { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy\\-MM\\-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Pvm")]
        public DateTime? Pvm { get; set; }

        public int? Esimies { get; set; }

        public string Status { get; set; }
        public DateTime? Avattu { get; set; }
        public DateTime? Suljettu { get; set; }

        public int? ProjektiTunti { get; set; }
        public decimal? ProjektiTunnit { get; set; }
        public decimal? SuunnitellutTunnit { get; set; }
        public decimal? ToteutuneetTunnit { get; set; }
        public string ProjektiNimi { get; set; }

        public virtual ICollection<SimplyProjektitData> SimplyProjektitData { get; set; }
        public virtual ICollection<SimplyHenkilotData> SimplyHenkilotData { get; set; }
    }
}