using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Models;

namespace LendingLibrary.Web.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly IPersonRepository _personRepository;

        public PeopleController(IMappingEngine mappingEngine, IPersonRepository personRepository)
        {
            if (mappingEngine == null) throw new ArgumentNullException("mappingEngine");
            if (personRepository == null) throw new ArgumentNullException("personRepository");
            _mappingEngine = mappingEngine;
            _personRepository = personRepository;
        }

        public ActionResult Index()
        {
            var people = _personRepository.GetAll();
            var personViewModels = new List<PersonViewModel>();
            if (people != null) personViewModels = _mappingEngine.Map<List<Person>, List<PersonViewModel>>(people);
            return View(personViewModels);
        }
//
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var person = db.People.Find(id);
//            if (person == null)
//            {
//                return HttpNotFound();
//            }
//            return View(person);
//        }
//
//        public ActionResult Create()
//        {
//            return View();
//        }
//
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(Person person)
//        {
//            if (ModelState.IsValid)
//            {
//                db.People.Add(person);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//
//            return View(person);
//        }
//
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var person = db.People.Find(id);
//            if (person == null)
//            {
//                return HttpNotFound();
//            }
//            return View(person);
//        }
//
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(Person person)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(person).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(person);
//        }
//
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var person = db.People.Find(id);
//            if (person == null)
//            {
//                return HttpNotFound();
//            }
//            return View(person);
//        }
//
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            var person = db.People.Find(id);
//            db.People.Remove(person);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }
//
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
    }
}
