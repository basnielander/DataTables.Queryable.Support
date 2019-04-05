using Datatables.Sample.Domain.Interfaces;
using Datatables.Sample.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Datatables.Sample.Domain.Managers
{
    public class ToDoManager : IToDoManager
    {
        private readonly IToDoRepository toDoRepository;

        public ToDoManager(IToDoRepository toDoRepository)
        {
            this.toDoRepository = toDoRepository;
        }
        public IOrderedQueryable<ToDoModel> SelectToDos()
        {
            return toDoRepository.SelectToDos();
        }
    }
}
