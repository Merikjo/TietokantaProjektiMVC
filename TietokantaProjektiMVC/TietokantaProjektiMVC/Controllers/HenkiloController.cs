using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TietokantaProjektiMVC.Models;
using Newtonsoft.Json;
using TietokantaProjekti2MVC.Utilities;


namespace TietokantaProjektiMVC.Controllers
{
    public class HenkiloController : Controller
    {
        // GET: Henkilo
        public ActionResult Index()
        {
            List<Henkilot> model = new List<Henkilot>();

            try
            {
                TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();
                model = entities.Henkilot.ToList();

                entities.Dispose();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.GetType() + ": " + ex.Message;
            }

            return View(model);
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult GetTunnit(int? id)
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            List<Tunnit> tunnit = (from t in entities.Tunnit
                                   where t.HenkiloID == id
                                   select t).ToList();

            List<SimplyTunnitData> result = new List<SimplyTunnitData>();

            CultureInfo fiFi = new CultureInfo("fi-FI");

            foreach (Tunnit tunti in tunnit)
            {
                SimplyTunnitData data = new SimplyTunnitData();

                data.TuntiID = tunti.TuntiID;
                data.HenkiloID = (int)(tunti.HenkiloID);
                data.Pvm = tunti.Pvm.Value;
                data.ProjektiTunnit = (int)tunti.ProjektiTunnit;

                List<Projektit> projektit = (from p in entities.Projektit
                                             where p.ProjektiID == tunti.ProjektiID
                                             select p).ToList();

                data.ProjektiNimi = projektit[0].ProjektiNimi;

                result.Add(data);
            }

            entities.Dispose();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePerson()
        {
            TietokantaProjektiDataEntities db = new TietokantaProjektiDataEntities();

            Henkilot model = new Henkilot();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePerson(Henkilot model)
        {
            TietokantaProjektiDataEntities db = new TietokantaProjektiDataEntities();

            Henkilot henkilot = new Henkilot();
            henkilot.HenkiloID = model.HenkiloID;
            henkilot.Etunimi = model.Etunimi;
            henkilot.Sukunimi = model.Sukunimi;
            henkilot.Osoite = model.Osoite;
            henkilot.Esimies = model.Esimies;
            henkilot.Postinumero = model.Postinumero;

            db.Henkilot.Add(henkilot);

            try
            {
                db.SaveChanges();
            }

            catch (Exception ex)
            {
            }

            return RedirectToAction("Index");
        }


        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Henkilot henkilo =TietokantaProjektiDataEntities.Henkilots.Find(id);
            if (henkilo == null)
            {
                return HttpNotFound();
            }
            return View(henkilo);*/


        //Dynaaminen: palauttava metodi
        public JsonResult GetList()
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            var model = (from h in entities.Henkilot
                         select new{
                             HenkiloID = h.HenkiloID,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Osoite = h.Osoite,
                             Esimies = (int)h.Esimies
                         }).ToList();


            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            //Välimuistin hallinta
            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleHenkilo(string id)
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            int henkiloid = int.Parse(id);

            //Näkymämalli eli näkymäluokka, siihen malliin, joka halutaan välittää kontrollerista näkymälle, 
            //tässä tapauksessa Ajaxilla.
            //rajoitetaan anonyymin tietotyypin dataa, kun käytetään taulujen välisiä kytkentöjä:
            var model = (from h in entities.Henkilot join t in entities.Tunnit on h.HenkiloID equals t.HenkiloID
                         where h.HenkiloID == henkiloid //haetaan vain yksi tieto where lausekkeella
                         select new
                         {
                             HenkiloID = h.HenkiloID,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Osoite = h.Osoite,
                             Esimies = (int)h.Esimies
                         }).FirstOrDefault(); //Haetaan vain yksi id tieto, joka voi olla myös null

            //Dispose käytön serialisointi = käsitellään vähemmän dataa
            string json = JsonConvert.SerializeObject(model);
            //Tietokannan vapautus
            entities.Dispose();

            //palautetaan merkkijono 'json' muodossa, kun käytetään serialisointia
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Henkilot henk)
        //käytetään Customer tyyppistä mallioliota tiedon välittämiseen
        {
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            CultureInfo fiFi = new CultureInfo("fi-FI");

            //oletetaan että tallennusoperaatio ei onnistu
            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            //if (id == "(uusi)")
            if (henk.HenkiloID == 0)
            //if (id == null)
            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Henkilot dbItem = new Henkilot()
                {
                    //HenkiloID = henk.HenkiloID,
                    Etunimi = henk.Etunimi,
                    Sukunimi = henk.Sukunimi,
                    Osoite = henk.Osoite,
                    Esimies = henk.Esimies
                };

                // tallennus tietokantaan
                entities.Henkilot.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                //haetaan id:n perusteella rivi SQL tietokannasta
                Henkilot dbItem = (from h in entities.Henkilot
                                   where h.HenkiloID == henk.HenkiloID
                                   select h).FirstOrDefault(); //haetaan vain yhden henkilön tiedot

                //jos tiedot löytyvät eli ei ole null
                if (dbItem != null)
                {
                    //dbItem.HenkiloID = henk.HenkiloID;  //tätä ei käytetä
                    dbItem.Etunimi = henk.Etunimi;
                    dbItem.Sukunimi = henk.Sukunimi;
                    dbItem.Osoite = henk.Osoite;
                    dbItem.Esimies = henk.Esimies;


                    // tallennus SQL tietokantaan
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

            int henkiloid = int.Parse(id);

            // haetaan id:n perusteella asiakasrivi SQL tietokannasta
            bool OK = false;

            Henkilot dbItem = (from h in entities.Henkilot
                               where h.HenkiloID == henkiloid
                               select h).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Henkilot.Remove(dbItem);
                entities.SaveChanges();

                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        //VaihtoehtoB, tietojan tallentaminen SQL tietokantaan:

        public ActionResult TuntiCreate()
        {
            List<SimplyTunnitData> model = new List<SimplyTunnitData>();

            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();
            try
            {
                List<Tunnit> tunnit = entities.Tunnit.ToList();

                CultureInfo fiFi = new CultureInfo("fi-FI");

                // muodostetaan näkymämalli tietokannan rivien pohjalta
                foreach (Tunnit tunti in tunnit)
                {
                    SimplyTunnitData view = new SimplyTunnitData();
                    view.TuntiID = tunti.TuntiID;
                    view.ProjektiID = tunti.Projektit?.ProjektiID;
                    view.HenkiloID = tunti.Henkilot?.HenkiloID;
                    view.Pvm = tunti.Pvm.Value;
                    view.ProjektiTunnit = tunti.ProjektiTunnit;
                    view.ProjektiNimi = tunti.Projektit?.ProjektiNimi;
                    view.Henkilonimi = tunti.Henkilot?.Etunimi + ": " + tunti.Henkilot?.Sukunimi;
                    //view.Pvm = tunti.Pvm.Value.ToString(fiFi);
                  

                    model.Add(view);
                }
            }
            finally
            {
                entities.Dispose();
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult GetHenkiloTunnit()
        {
            string json = Request.InputStream.ReadToEnd();
            SimplyTunnitData inputData =
                 JsonConvert.DeserializeObject<SimplyTunnitData>(json);

            bool success = false;
            string error = "";

            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            try
            {
                int tunnitId = (from t in entities.Tunnit
                          where t.ProjektiTunnit== inputData.ProjektiTunnit
                          select t.TuntiID).FirstOrDefault();

                int projektitId = (from t in entities.Projektit
                          where t.ProjektiID== inputData.ProjektiID
                          select t.ProjektiID).FirstOrDefault();

                int henkiloId = (from t in entities.Henkilot
                                   where t.HenkiloID == inputData.HenkiloID
                                   select t.HenkiloID).FirstOrDefault();

                if ((tunnitId != 0) && (projektitId > 0) && (henkiloId > 0))
                {
                    //tallennetaan uusi rivi aikaleiman kanssa kantaan:
                    Tunnit newEntry = new Tunnit();
                    newEntry.TuntiID = tunnitId;
                    newEntry.ProjektiID = projektitId;
                    newEntry.HenkiloID = henkiloId;
                    
                    newEntry.Pvm = DateTime.Now;

                    entities.Tunnit.Add(newEntry);

                    entities.SaveChanges();

                    success = true;
                }
            }
            catch (Exception ex)
            {
                    error = ex.GetType().Name + ": " + ex.Message;
            }
            finally
            {
                    entities.Dispose();
            }

            //palautetaan JSON-muotoinen tulos kutsujalle
            var result = new { success = success, error = error };
            return Json(result);

            }
        }
    }


