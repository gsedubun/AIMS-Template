﻿using System.Linq;
using AIMS.Core;
using AIMS.Core.DTO;
using AIMS.Core.Entities;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIMS.Web.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IRepository _repository;

        public ToDoController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var items = _repository.List<ToDoItem>()
                            .Select(ToDoItemDTO.FromToDoItem);
            return View(items);
        }

        public IActionResult Populate()
        {
            int recordsAdded = DatabasePopulator.PopulateDatabase(_repository);
            return Ok(recordsAdded);
        }
    }
}
