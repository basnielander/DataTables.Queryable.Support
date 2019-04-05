using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Datatables.Sample.Domain.Interfaces;
using Datatables.Sample.Domain.Models;
using Datatables.Samples.Models;
using DataTables.Sample.Data.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Datatables.Sample.Data.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly IMapper mapper;

        public ToDoRepository(DataTablesContext context, IMapper mapper)
        {
            Context = context;
            this.mapper = mapper;
        }

        public DataTablesContext Context { get; }

        public IOrderedQueryable<ToDoModel> SelectToDos()
        {
            IQueryable<ToDo> items = Context.ToDos.Select(item => item);
            return items.UseAsDataSource(mapper).For<ToDoModel>() as IOrderedQueryable<ToDoModel>;
        }
    }
}
