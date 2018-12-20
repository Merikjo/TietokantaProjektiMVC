using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TietokantaProjektiMVC.Models;
using Newtonsoft.Json;


namespace TietokantaProjektiMVC.Controllers
{
    public class ProjektiController : Controller
    {
        private TietokantaProjektiDataEntities db = new TietokantaProjektiDataEntities();

        // GET: Projekti
        public ActionResult Index()
        {
           TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();
            List<Projektit> model = entities.Projektit.ToList();
            entities.Dispose();

            return View(model);
        }
        public ActionResult Index2()
        {
            return View();
        }

            public ActionResult GetProjektiTunnit(int? id)
        {
           TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            List<Tunnit> tunnit = (from t in entities.Tunnit
                                   where t.ProjektiID == id
                                   select t).ToList();

            List<SimplyTunnitData> result = new List<SimplyTunnitData>();

            CultureInfo fiFi = new CultureInfo("fi-FI");

            foreach (Tunnit tunti in tunnit)
            {
                SimplyTunnitData data = new SimplyTunnitData();

                data.HenkiloID = (int)tunti?.HenkiloID;
                data.ProjektiID = (int)tunti.ProjektiID;
                data.TuntiID = tunti.TuntiID;

                List<Henkilot> henkilot = (from p in entities.Henkilot
                                           where p.HenkiloID == tunti.HenkiloID
                                           select p).ToList();

                data.Etunimi = henkilot[0].Etunimi;
                data.Sukunimi = henkilot[0].Sukunimi;

                data.Pvm = tunti.Pvm.Value;
                data.ProjektiTunti = tunti.ProjektiTunti;

                result.Add(data);
            }
            entities.Dispose();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateProject()
        {
           TietokantaProjektiDataEntities db = new TietokantaProjektiDataEntities();

            Projektit model = new Projektit();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProject(Projektit model)
        {
           TietokantaProjektiDataEntities db = new TietokantaProjektiDataEntities();

            Projektit projektit = new Projektit();
            projektit.ProjektiID = model.ProjektiID;
            projektit.ProjektiNimi = model.ProjektiNimi;

            db.Projektit.Add(projektit);

            try
            {
                db.SaveChanges();
            }

            catch (Exception ex)
            {
            }

            return RedirectToAction("Index");
        }


        //DYNAAMINEN: palauttava metodi
        public JsonResult GetList()
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            //Näkymämalli eli näkymäluokka, siihen malliin, joka halutaan välittää kontrollerista näkymälle, 
            //tässä tapauksessa Ajaxilla.
            //rajoitetaan anonyymin tietotyypin dataa, kun käytetään taulujen välisiä kytkentöjä:
            var model = (from p in entities.Projektit
                         select new{
                             ProjektiID = (int)p.ProjektiID,
                             ProjektiNimi = p.ProjektiNimi,
                             Status = p.Status,
                         }).ToList();

            //Dispose käytön serialisointi = käsitellään vähemmän dataa
            string json = JsonConvert.SerializeObject(model);

            //Tietokannan vapautus
            entities.Dispose();

            //Välimuistin hallinta
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            //palautetaan merkkijono 'json' muodossa, kun käytetään serialisointia
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleProjektit(string id)
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            int projektiid = int.Parse(id);

            var model = (from p in entities.Projektit
                         where p.ProjektiID == projektiid
                         select new
                         {
                             ProjektiID = p.ProjektiID,
                             ProjektiNimi = p.ProjektiNimi,
                             Status = p.Status
                         }).FirstOrDefault(); //Haetaan vain yksi id tieto, joka voi olla myös null

            //Dispose käytön serialisointi = käsitellään vähemmän dataa
            string json = JsonConvert.SerializeObject(model);
            //Tietokannan vapautus
            entities.Dispose();

            //palautetaan merkkijono 'json' muodossa, kun käytetään serialisointia
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Update(Projektit pro)
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            CultureInfo fiFi = new CultureInfo("fi-FI");

            //oletetaan että tallennusoperaatio ei onnistu
            bool OK = false;

            // tiedon muokkaus tai uuden tiedon lisääminen tietokantaan
            if (pro.ProjektiID == 0)
            //if (id == null)
                {
                // kyseessä on uuden projektitn lisääminen, kopioidaan kentät
                Projektit dbItem = new Projektit()
                {
                    //ProjektiID = pro.ProjektiID,
                    ProjektiNimi = pro.ProjektiNimi,
                    Status = pro.Status
                };

                // Tallennus SQL tietokantaan
                entities.Projektit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                //Haetaan id:n perusteella riviä SQL tietokannasta
                Projektit dbItem = (from p in entities.Projektit
                                    where p.ProjektiID == pro.ProjektiID
                                    select p).FirstOrDefault(); //haetaan vain yhden projektin tiedot
                
                //jos tiedot löytyvät eli ei ole null
                if (dbItem != null)
                {
                    //dbItem.ProjektiID = pro.ProjektiID;
                    dbItem.ProjektiNimi = pro.ProjektiNimi;
                    dbItem.Status = pro.Status;

                    // tallennetaan tiedot SQL tietokantaan
                    entities.SaveChanges();

                    //jos tallennus onnistuu
                    OK = true;
                }
            }
            //entiteettiolion vapauttaminen
            entities.Dispose();

            // palautetaan 'json' muodossa
            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string id) //Delete = rutiini
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            int projektiid = int.Parse(id);

            // haetaan id:n perusteella asiakasrivi kannasta
            bool OK = false;

            Projektit dbItem = (from p in entities.Projektit
                                where p.ProjektiID == projektiid
                                select p).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Projektit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }
    }
}
         