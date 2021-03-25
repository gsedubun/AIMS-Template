using AIMS.Core.DTO;
using AIMS.Core.Entities;
using AIMS.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AIMS.Core.Services
{
    public sealed class TodoItemServices
    {
        private IRepository _repository;

        public TodoItemServices(IRepository Repository)
        {
            _repository = Repository;
        }

        public ToDoItemDTO AddTodoItem(ToDoItemDTO toDoItem)
        {
            ToDoItem todo = new ToDoItem() { Description = toDoItem.Description, Id = toDoItem.Id, Title = toDoItem.Title };

            todo = _repository.Add(todo);
            return ToDoItemDTO.FromToDoItem(todo);
        }

        public ToDoItemDTO Update(ToDoItemDTO toDoItem)
        {
            ToDoItem todo = new ToDoItem() { Description = toDoItem.Description, Id = toDoItem.Id, Title = toDoItem.Title };

            _repository.Update(todo);
            var res = ToDoItemDTO.FromToDoItem(todo);
            return res;
        }
        public ToDoItemDTO MarkComplete(int id)
        {
            ToDoItem todo = _repository.GetById<ToDoItem>(id);

            todo.MarkComplete();
            _repository.Update(todo);
            var res = ToDoItemDTO.FromToDoItem(todo);
            return res;
        }
        public ToDoItemDTO GetById(int id)
        {

            ToDoItem todo  = _repository.GetById<ToDoItem>(id);
            

            var res = ToDoItemDTO.FromToDoItem(todo);
            return res;
        }
        public IEnumerable<ToDoItemDTO> GetAll()
        {
            var res= _repository.List<ToDoItem>()
                            .Select(ToDoItemDTO.FromToDoItem);
            return res;
        }
    }
}
