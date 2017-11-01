using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FortuneTellerMVC.Models;

namespace FortuneTellerMVC.Controllers
{
    public class CustomersController : Controller
    {
        private FortuneTellerMVCEntities db = new FortuneTellerMVCEntities();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.BirthMonth).Include(c => c.FavoriteColor);
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            //Retirement age based on customer age
            if (customer.Age % 2 == 0)
            {
                ViewBag.RetirementAge = "15 years";
            }
            else if (customer.Age %2 == 1)
            {
                ViewBag.RetirementAge = "50 years";
            }
            else
            {
                ViewBag.RetirementAge = "never";
            }

            //Vacation Home based on number of siblings
           if (customer.NumberOfSiblings == 0)
            {
                ViewBag.VacationHome = "Hawaii";
            }
            else if (customer.NumberOfSiblings == 1)
            {
                ViewBag.VacationHome = "Florida";
            }
            else if (customer.NumberOfSiblings == 2)
            {
                ViewBag.VacationHome = "Thailand";
            }
            else if (customer.NumberOfSiblings == 3)
            {
                ViewBag.VacationHome = "Paris";
            }
            else if (customer.NumberOfSiblings > 3)
            {
                ViewBag.VacationHome = "Greece";
            }
            else
            {
                ViewBag.VacationHome = "Dumpster";
            }

            // Money in the bank based on birth month
            if (customer.BirthMonthID > 0 && customer.BirthMonthID <= 4)
            {
                ViewBag.BankMoney = 1000000;
            }
            else if (customer.BirthMonthID > 4 && customer.BirthMonthID <= 8)
            {
                ViewBag.BankMoney = 500000;
            }
            else if (customer.BirthMonthID > 8 && customer.BirthMonthID <= 12)
            {
                ViewBag.BankMoney = 250000;
            }
            else
            {
                ViewBag.BankMoney = 0;
            }

            //Transportation based on favorite color

            if (customer.FavoriteColorID == 1)
            {
                ViewBag.RideType = "jetpack";
            }
            else if (customer.FavoriteColorID == 2)
            {
                ViewBag.RideType = "personal helicopter";
            }
            else if (customer.FavoriteColorID == 3)
            {
                ViewBag.RideType = "sports car";
            }
            else if (customer.FavoriteColorID == 4)
            {
                ViewBag.RideType = "speedboat";
            }
            else if (customer.FavoriteColorID == 5)
            {
                ViewBag.RideType = "motorcycle";
            }
            else if (customer.FavoriteColorID == 6)
            {
                ViewBag.RideType = "segway";
            }
            else if (customer.FavoriteColorID == 7)
            {
                ViewBag.RideType = "submarine";
            }

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "BirthMonth1");
            ViewBag.FavoriteColorID = new SelectList(db.FavoriteColors, "FavoriteColorID", "FavoriteColor1");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,FirstName,LastName,Age,BirthMonthID,FavoriteColorID,NumberOfSiblings")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "BirthMonth1", customer.BirthMonthID);
            ViewBag.FavoriteColorID = new SelectList(db.FavoriteColors, "FavoriteColorID", "FavoriteColor1", customer.FavoriteColorID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "BirthMonth1", customer.BirthMonthID);
            ViewBag.FavoriteColorID = new SelectList(db.FavoriteColors, "FavoriteColorID", "FavoriteColor1", customer.FavoriteColorID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,FirstName,LastName,Age,BirthMonthID,FavoriteColorID,NumberOfSiblings")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BirthMonthID = new SelectList(db.BirthMonths, "BirthMonthID", "BirthMonth1", customer.BirthMonthID);
            ViewBag.FavoriteColorID = new SelectList(db.FavoriteColors, "FavoriteColorID", "FavoriteColor1", customer.FavoriteColorID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
