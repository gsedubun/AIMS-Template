using AIMS.Core.DTO;
using AIMS.Core.Entities;
using AIMS.Core.Services;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AIMS.Web.Api
{
    public class ToDoItemsController : BaseApiController
    {
        private readonly IRepository _repository;
        private readonly TodoItemServices _services;

        public ToDoItemsController(IRepository repository, TodoItemServices services)
        {
            _repository = repository;
            _services = services;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public IActionResult List()
        {
            //var items = _repository.List<ToDoItem>()
            //                .Select(ToDoItemDTO.FromToDoItem);
            var items = _services.GetAll();
            return Ok(items);
        }

        // GET: api/ToDoItems
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            //var item = ToDoItemDTO.FromToDoItem(_repository.GetById<ToDoItem>(id));
            //return Ok(item);

            var item = _services.GetById(id);
            return Ok(item);
        }

        // POST: api/ToDoItems
        [HttpPost]
        public IActionResult Post([FromBody] ToDoItemDTO item)
        {
            //var todoItem = new ToDoItem()
            //{
            //    Title = item.Title,
            //    Description = item.Description
            //};
            //_repository.Add(todoItem);
            //return Ok(ToDoItemDTO.FromToDoItem(todoItem));

            ToDoItemDTO data =  _services.AddTodoItem(item);
            return Ok(data);
        }

        [HttpPatch("{id:int}/complete")]
        public IActionResult Complete(int id)
        {
            //var toDoItem = _repository.GetById<ToDoItem>(id);
            //toDoItem.MarkComplete();
            //_repository.Update(toDoItem);

            //return Ok(ToDoItemDTO.FromToDoItem(toDoItem));

            var todoitem  = _services.MarkComplete(id);
            return Ok(todoitem);
        }
    }
}
