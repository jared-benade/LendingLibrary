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
    }
}