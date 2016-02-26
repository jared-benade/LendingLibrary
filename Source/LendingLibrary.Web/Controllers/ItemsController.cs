using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Models;

namespace LendingLibrary.Web.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IItemTypeRepository _itemTypeRepository;
        private readonly IMappingEngine _mappingEngine;

        public ItemsController(IItemRepository itemRepository, IMappingEngine mappingEngine,
            IItemTypeRepository itemTypeRepository)
        {
            if (itemRepository == null) throw new ArgumentNullException(nameof(itemRepository));
            if (mappingEngine == null) throw new ArgumentNullException(nameof(mappingEngine));
            if (itemTypeRepository == null) throw new ArgumentNullException(nameof(itemTypeRepository));
            _itemRepository = itemRepository;
            _mappingEngine = mappingEngine;
            _itemTypeRepository = itemTypeRepository;
        }

        public ActionResult Index()
        {
            var itemViewModels = new List<ItemViewModel>();
            var items = _itemRepository.GetAllActive();
            if (items != null)
                itemViewModels = _mappingEngine.Map<List<Item>, List<ItemViewModel>>(items);
            return View(itemViewModels);
        }

        public ActionResult Create()
        {
            var viewModel = new ItemViewModel();
            var itemTypeSelectList = GetItemTypeSelectList(viewModel);
            viewModel.ItemTypeSelectList = itemTypeSelectList;
            viewModel.IsActive = true;
            viewModel.Available = true;

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var item = _mappingEngine.Map<ItemViewModel, Item>(viewModel);
                if (item != null) _itemRepository.Save(item);
                return RedirectToAction("Index");
            }
            var itemTypeSelectList = GetItemTypeSelectList(viewModel);
            viewModel.ItemTypeSelectList = itemTypeSelectList;

            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var viewModel = new ItemViewModel();
            var item = _itemRepository.GetById(id);
            if (item != null) viewModel = _mappingEngine.Map<Item, ItemViewModel>(item);
            if (viewModel != null)
            {
                var itemTypeSelectList = GetItemTypeSelectList(viewModel);
                viewModel.ItemTypeSelectList = itemTypeSelectList;
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var item = _mappingEngine.Map<ItemViewModel, Item>(viewModel);
                if (item != null) _itemRepository.Save(item);
                return RedirectToAction("Index", "Items");
            }
            var itemTypeSelectList = GetItemTypeSelectList(viewModel);
            viewModel.ItemTypeSelectList = itemTypeSelectList;
            return View(viewModel);
        }

        public JsonResult Delete(int id)
        {
            if (id != 0) _itemRepository.DeleteById(id);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        private SelectList GetItemTypeSelectList(ItemViewModel viewModel)
        {
            var itemTypes = _itemTypeRepository.GetAllActive() ?? new List<ItemType>();

            var listItems = itemTypes.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = x.Id.ToString()
            });

            var itemTypeSelectList = new SelectList(listItems, "Value", "Text", viewModel.ItemTypeId);
            return itemTypeSelectList;
        }
    }
}