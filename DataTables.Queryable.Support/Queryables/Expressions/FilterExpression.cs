using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class FilterExpression<TModel>
    {
        public FilterExpression(IColumn column, ISearch search, Expression<Func<TModel, bool>> expression)
        {
            Column = column;
            Search = search;
            Expression = expression;
        }

        public IColumn Column { get; }
        public ISearch Search { get; }
        public Expression<Func<TModel, bool>> Expression { get; set; }
    }
}
