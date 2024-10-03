using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // POST: Insuree Quote
        [HttpPost]
        public ActionResult Quote(int Id)
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insuree = new Insuree();
                DateTime dateOfBirth = insuree.DateOfBirth;
                int age = (DateTime.Now.Year - dateOfBirth.Year);
                int carYear = insuree.CarYear;
                string carMake = insuree.CarMake;
                string carModel = insuree.CarModel;
                int speedingTickets = insuree.SpeedingTickets;
                bool dui = insuree.DUI;
                bool coverageType = insuree.CoverageType;
                decimal quote = insuree.Quote;

                const decimal baseQuote = 50.00M;

                if (dui)
                {
                    if (age <= 18)
                    {
                        if (carYear < 2000 || carYear > 2015)
                        {
                            if (carMake == "Porsche")
                            {
                                if (carModel == "911 Carrera")
                                {
                                    quote = baseQuote + 25.00M;
                                }
                                else
                                {
                                    quote = baseQuote;
                                }
                                quote = quote + 25.00M;
                            }
                            quote = quote + 25.00M;
                        }
                        quote = quote + 100.00M + (10.00M * speedingTickets);

                    }
                    else if (age >= 19 && age <= 25)
                    {
                        quote = quote + 50.00M + (10.00M * speedingTickets);
                    }
                    else
                    {
                        quote = quote + 25.00M + (10.00M * speedingTickets);
                    }
                    quote = 1.25M * quote;
                    insuree.Quote = Math.Round(quote, 2);
                }               
                return View(insuree);
            }
            
        }
        public ActionResult Admin()
        {
            //Data is from Database
            var insureeVm = new Insuree();
            var InsureeVms = new List<Insuree>();
            foreach (var insuree in db.Insurees)
            {
                insureeVm.FirstName = insuree.FirstName;
                insureeVm.LastName = insuree.LastName;
                insureeVm.EmailAddress = insuree.EmailAddress;
                insureeVm.Quote = insuree.Quote;
                InsureeVms.Add(insureeVm);
            }
            return View(InsureeVms);            
        }
        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
