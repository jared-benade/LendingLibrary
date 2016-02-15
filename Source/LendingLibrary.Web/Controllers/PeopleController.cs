using System;
using System.Collections.Generic;
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
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            if (personRepository == null) throw new ArgumentNullException(nameof(personRepository));
            _mappingEngine = mappingEngine;
            _personRepository = personRepository;
        }

        public ActionResult Index()
        {
            var people = _personRepository.GetAllActivePeople();
            var personViewModels = new List<PersonViewModel>();
            if (people != null) personViewModels = _mappingEngine.Map<List<Person>, List<PersonViewModel>>(people);
            return View(personViewModels);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var person = _mappingEngine.Map<PersonViewModel, Person>(viewModel);
                if (person != null)
                {
                    person.IsActive = true;
                    _personRepository.Save(person);
                }
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var person = _personRepository.GetById(id);
            var viewModel = _mappingEngine.Map<Person, PersonViewModel>(person);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var person = _mappingEngine.Map<PersonViewModel, Person>(viewModel);
                _personRepository.Save(person);
                return RedirectToAction("Index", "People");
            }
            return View(viewModel);
        }

        public JsonResult Delete(int id)
        {
            if(id != 0) _personRepository.DeleteById(id);
            return Json("",JsonRequestBehavior.AllowGet);
        }
    }
}
