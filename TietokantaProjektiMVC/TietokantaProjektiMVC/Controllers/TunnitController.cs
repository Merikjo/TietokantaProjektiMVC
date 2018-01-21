using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TietokantaProjekti2MVC.Models;
using TietokantaProjektiMVC.Models;

namespace TietokantaProjekti2MVC.Controllers
{
    public class TunnitController : Controller
    {
        private TietokantaProjektiDataEntities db = new TietokantaProjektiDataEntities();
        // GET: Tunti
        public ActionResult Index()
        {
            List<SimplyTunnitData> model = new List<SimplyTunnitData>();
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            try
            {
                List<Tunnit> tunnit = entities.Tunnit.OrderByDescending(Tunnit => Tunnit.Pvm).ToList();

                CultureInfo fiFi = new CultureInfo("fi-FI");

                // muodostetaan näkymämalli tietokannan rivien pohjalta
                foreach (Tunnit tunti in tunnit)
                {
                    SimplyTunnitData tun = new SimplyTunnitData();
                    tun.TuntiID = tunti.TuntiID;
                    tun.Pvm = tunti.Pvm.Value;
                    tun.ProjektiTunti = tunti.ProjektiTunti;
                    tun.ProjektiTunnit = (decimal)(tunti.ProjektiTunnit);
                    //tun.SuunnitellutTunnit = tunti.SuunnitellutTunnit;
                    //tun.ToteutuneetTunnit = tunti.ToteutuneetTunnit;

                    tun.HenkiloID = tunti.Henkilot.HenkiloID;
                    tun.Henkilonimi = tunti.Henkilot?.Etunimi + " " + tunti.Henkilot?.Sukunimi;

                    tun.ProjektiID = tunti.Projektit.ProjektiID;
                    tun.ProjektiNimi = tunti.Projektit.ProjektiNimi;
                    //tun.Avattu = tunti.Projektit?.Avattu;
                    //tun.Suljettu = tunti.Projektit?.Suljettu;
                    //tun.Status = tunti.Projektit?.Status;

                    model.Add(tun);
                }

                return View(model);
            }

            finally
            {
                entities.Dispose();
            }
        }

        CultureInfo fiFi = new CultureInfo("fi-FI");

        // GET: Tunti/Details/5
        public ActionResult Details(int? id)
        {
            SimplyTunnitData model = new SimplyTunnitData();
            TietokantaProjektiDataEntities entities = new TietokantaProjektiDataEntities();

            CultureInfo fiFi = new CultureInfo("fi-FI");

            try
            {
                Tunnit tunti = entities.Tunnit.Find(id);
                if (tunti == null)
                {
                    return HttpNotFound();
                }

                Tunnit tuntidetail = entities.Tunnit.Find(tunti.TuntiID);

                SimplyTunnitData tun = new SimplyTunnitData();
                tun.TuntiID = tuntidetail.TuntiID;
                tun.Pvm = tuntidetail.Pvm.GetValueOrDefault();
                tun.ProjektiTunti = tuntidetail.ProjektiTunti.GetValueOrDefault();
                tun.ProjektiTunnit = (decimal)(tuntidetail.ProjektiTunnit);
                //tun.SuunnitellutTunnit = tuntidetail.SuunnitellutTunnit;
                //tun.ToteutuneetTunnit = tuntidetail.ToteutuneetTunnit;

                tun.HenkiloID = tuntidetail.Henkilot.HenkiloID;
                tun.Etunimi = tuntidetail.Henkilot.Etunimi;
                tun.Sukunimi = tuntidetail.Henkilot.Sukunimi;
                tun.Henkilonimi = tunti.Henkilot?.Etunimi + " " + tunti.Henkilot?.Sukunimi;

                tun.ProjektiID = tuntidetail.Projektit.ProjektiID;
                tun.ProjektiNimi = tuntidetail.Projektit.ProjektiNimi;
                //tun.Avattu = tuntidetail.Projektit.Avattu;
                //tun.Suljettu = tuntidetail.Projektit.Suljettu;
                //tun.Status = tuntidetail.Projektit.Status;

                model = tun;
            }
            finally
            {
                entities.Dispose();
            }

            return View(model);
        }
        public ActionResult GetTunnit(int? id)
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

                data.HenkiloID = (int)(tunti.HenkiloID);
                data.ProjektiID = (int)tunti.ProjektiID;
                data.TuntiID = tunti.TuntiID;
                //data.Pvm = tunti.Pvm.Value.ToString(fiFi);
                data.Pvm = tunti.Pvm.Value;
                data.ProjektiTunnit = (decimal)(tunti.ProjektiTunti);

                List<Henkilot> henkilot = (from p in entities.Henkilot
                                           where p.HenkiloID == tunti.HenkiloID
                                           select p).ToList();

                data.Etunimi = henkilot[0].Etunimi;
                data.Sukunimi = henkilot[0].Sukunimi;
                //data.Henkilonimi = henkilot[0].HenkiloID;

                List<Projektit> projektit = (from p in entities.Projektit
                                             where p.ProjektiID == tunti.ProjektiID
                                             select p).ToList();
                data.ProjektiNimi = projektit[0].ProjektiNimi;

                result.Add(data);
            }
            entities.Dispose();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            TietokantaProjektiDataEntities db = new TietokantaProjektiDataEntities();

            SimplyTunnitData model = new SimplyTunnitData();

            //ViewBag.ProjektiNimi = new SelectList((from pn in db.Projektit select new { Projekti_id = pn.Projekti_id, ProjektiNimi = pn.ProjektiNimi }), "Projekti_id", "ProjektiNimi", null);
            //ViewBag.Sukunimi = new SelectList((from kn in db.Henkilot select new { Henkilo_id = kn.Henkilo_id, Sukunimi = kn.Sukunimi }), "Henkilo_id", "Sukunimi", null);

            ViewBag.HenkiloID = new SelectList(db.Henkilot, "HenkiloID", "Etunimi");
            ViewBag.ProjektiID = new SelectList(db.Projektit, "ProjektiID", "ProjektiNimi");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SimplyTunnitData model)
        {
            Tunnit tunnit = new Tunnit();
            tunnit.TuntiID = model.TuntiID;
            tunnit.ProjektiID = model.ProjektiID;
            tunnit.HenkiloID = model.HenkiloID;
            tunnit.Pvm = DateTime.Now;
            tunnit.ProjektiTunti = model.ProjektiTunti;
            //tunnit.ProjektiTunnit = (decimal)(model.ProjektiTunnit);

            db.Tunnit.Add(tunnit);

            int henkiloId = int.Parse(model.Etunimi);
            if (henkiloId > 0)
            {
                Tunnit tun = db.Tunnit.Find(henkiloId);
                tunnit.HenkiloID = tun.HenkiloID;
            }

            int projektiId = int.Parse(model.ProjektiNimi);
            if (projektiId > 0)
            {
                Projektit pro = db.Projektit.Find(projektiId);
                tunnit.ProjektiID = pro.ProjektiID;
            }

            //ViewBag.Sukunimi = new SelectList((from kn in db.Henkilot select new { Henkilo_id = kn.Henkilo_id, Sukunimi = kn.Sukunimi }), "Henkilo_id", "Sukunimi", tunnit.Henkilo_id);
            //ViewBag.ProjektiNimi = new SelectList((from pn in db.Projektit select new { Projekti_id = pn.Projekti_id, ProjektiNimi = pn.ProjektiNimi }), "Projekti_id", "ProjektiNimi", tunnit.Projekti_id);

            ViewBag.HenkiloID = new SelectList(db.Henkilot, "HenkiloID", "Etunimi", tunnit.HenkiloID);
            ViewBag.ProjektiID = new SelectList(db.Projektit, "ProjektiID", "ProjektiNimi", tunnit.ProjektiID);

            try
            {
                db.SaveChanges();
            }

            catch (Exception ex)
            {
            }

            return RedirectToAction("Index");
        }

        // GET: Tunnit/Edit/5
        public ActionResult Edit(int? id)
        {
            CultureInfo fiFi = new CultureInfo("fi-FI");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tunnit tunti = db.Tunnit.Find(id);
            if (tunti == null)
            {
                return HttpNotFound();
            }

            SimplyTunnitData tun = new SimplyTunnitData();
            tun.TuntiID = tunti.TuntiID;
            tun.Pvm = tunti.Pvm.Value;
            tun.ProjektiTunti = tunti.ProjektiTunti;
            tun.ProjektiTunnit = (decimal)(tunti.ProjektiTunnit);
            //tun.SuunnitellutTunnit = tunti.SuunnitellutTunnit;
            //tun.ToteutuneetTunnit = tunti.ToteutuneetTunnit;

            tun.HenkiloID = tunti.Henkilot.HenkiloID;
            tun.Etunimi = tunti.Henkilot.Etunimi;
            tun.Sukunimi = tunti.Henkilot?.Sukunimi;
            //ViewBag.Sukunimi = new SelectList((from h in db.Henkilot select new { Henkilo_id = h.Henkilo_id, Sukunimi = h.Sukunimi }), "Henkilo_id", "Sukunimi", tun.Henkilo_id);

            tun.ProjektiID = tunti.Projektit.ProjektiID;
            tun.ProjektiNimi = tunti.Projektit.ProjektiNimi;
            //tun.Avattu = tunti.Projektit?.Avattu;
            //tun.Suljettu = tunti.Projektit?.Suljettu;
            //tun.Status = tunti.Projektit?.Status;
            //ViewBag.ProjektiNimi = new SelectList((from pn in db.Projektit select new { Projekti_id = pn.Projekti_id, ProjektiNimi = pn.ProjektiNimi }), "Projekti_id", "ProjektiNimi", tun.Projekti_id);

            ViewBag.HenkiloID = new SelectList(db.Henkilot, "HenkiloID", "Etunimi", tun.HenkiloID);
            ViewBag.ProjektiID = new SelectList(db.Projektit, "ProjektiID", "ProjektiNimi", tun.ProjektiID);

            return View(tun);
        }

        // POST: Tunnit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SimplyTunnitData model)
        {
            Tunnit tunnit = db.Tunnit.Find(model.TuntiID);
            //tunnit.TuntiID = model.TuntiID;
            tunnit.ProjektiID = model.ProjektiID;
            tunnit.HenkiloID = model.HenkiloID;
            tunnit.Pvm = model.Pvm.GetValueOrDefault();
            tunnit.ProjektiTunti = model.ProjektiTunti;
            tunnit.ProjektiTunnit = model.ProjektiTunnit;
            //tunnit.SuunnitellutTunnit = model.SuunnitellutTunnit;
            //tunnit.ToteutuneetTunnit = model.ToteutuneetTunnit;

            int henkiloId = int.Parse(model.Etunimi);
            if (henkiloId > 0)
            {
                Henkilot hlo = db.Henkilot.Find(henkiloId);
                tunnit.HenkiloID = hlo.HenkiloID;
            }

            int projektiId = int.Parse(model.ProjektiNimi);
            if (projektiId > 0)
            {
                Projektit pro = db.Projektit.Find(projektiId);
                tunnit.ProjektiID = pro.ProjektiID;
            }

            //ViewBag.ProjektiNimi = new SelectList((from pn in db.Projektit select new { Projekti_id = pn.Projekti_id, ProjektiNimi = pn.ProjektiNimi }), "Projekti_id", "ProjektiNimi", tunnit.Projekti_id);
            //ViewBag.Sukunimi = new SelectList((from h in db.Henkilot select new { Henkilo_id = h.Henkilo_id, Sukunimi = h.Sukunimi }), "Henkilo_id", "Sukunimi", tunnit.Henkilo_id);

            ViewBag.HenkiloID = new SelectList(db.Henkilot, "HenkiloID", "Etunimi", tunnit.HenkiloID);
            ViewBag.ProjektiID = new SelectList(db.Projektit, "ProjektiID", "ProjektiNimi", tunnit.ProjektiID);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        // GET: Tunnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tunnit tunti = db.Tunnit.Find(id);
            if (tunti == null)
            {
                return HttpNotFound();
            }

            SimplyTunnitData tun = new SimplyTunnitData();
            tun.TuntiID = tunti.TuntiID;
            tun.Pvm = tunti.Pvm.Value;
            tun.ProjektiTunti = tunti.ProjektiTunti;
            //tun.SuunnitellutTunnit = tunti.SuunnitellutTunnit;
            //tun.ToteutuneetTunnit = tunti.ToteutuneetTunnit;

            tun.HenkiloID = tunti.Henkilot.HenkiloID;
            tun.Etunimi = tunti.Henkilot?.Etunimi;
            tun.Sukunimi = tunti.Henkilot?.Sukunimi;

            tun.ProjektiID = tunti.Projektit.ProjektiID;
            tun.ProjektiNimi = tunti.Projektit?.ProjektiNimi;
            //tun.Avattu = tunti.Projektit?.Avattu;
            //tun.Suljettu = tunti.Projektit?.Suljettu;
            //tun.Status = tunti.Projektit?.Status;

            return View(tun);
        }

        // POST: Tunnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tunnit tunnit = db.Tunnit.Find(id);
            db.Tunnit.Remove(tunnit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

