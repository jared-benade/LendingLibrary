using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Models;

namespace LendingLibrary.Web.Controllers
{
    public class LendingTransactionController : Controller
    {
        private readonly ILendingTransactionRepository _lendingTransactionRepository;
        private readonly IMappingEngine _mappingEngine;

        public LendingTransactionController(ILendingTransactionRepository lendingTransactionRepository, IMappingEngine mappingEngine)
        {
            if (lendingTransactionRepository == null)
                throw new ArgumentNullException("lendingTransactionRepository");
            if (mappingEngine == null) throw new ArgumentNullException("mappingEngine");
            _lendingTransactionRepository = lendingTransactionRepository;
            _mappingEngine = mappingEngine;
        }

        public ActionResult Index()
        {
            var viewModels = new List<LendingTransactionViewModel>();

            var lendingTransactions = _lendingTransactionRepository.GetAllActive();
            if (lendingTransactions != null)
                viewModels = _mappingEngine.Map<List<LendingTransaction>, List<LendingTransactionViewModel>>(lendingTransactions);

            return View(viewModels);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LendingTransactionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var lendingTransaction = _mappingEngine.Map<LendingTransactionViewModel, LendingTransaction>(viewModel);
                if (lendingTransaction != null)
                {
                    lendingTransaction.IsActive = true;
                    _lendingTransactionRepository.Save(lendingTransaction);
                }
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var lendingTransaction = _lendingTransactionRepository.GetById(id);
            var viewModel = _mappingEngine.Map<LendingTransaction, LendingTransactionViewModel>(lendingTransaction);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LendingTransactionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var lendingTransaction = _mappingEngine.Map<LendingTransactionViewModel, LendingTransaction>(viewModel);
                _lendingTransactionRepository.Save(lendingTransaction);
                return RedirectToAction("Index", "LendingTransaction");
            }
            return View(viewModel);
        }

        public JsonResult Delete(int id)
        {
            if (id != 0) _lendingTransactionRepository.DeleteById(id);
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}