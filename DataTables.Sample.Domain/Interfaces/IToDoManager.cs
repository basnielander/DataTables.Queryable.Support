using Datatables.Sample.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Datatables.Sample.Domain.Interfaces
{
    public interface IToDoManager
    {
        IQueryable<ToDoModel> SelectToDos();
    }
}
