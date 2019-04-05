using DataTables.AspNet.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class OrderExpression<TModel>
    {
        public OrderExpression(IColumn column, ISort sortDetails, Expression<Func<TModel, object>> expression)
        {
            Column = column;
            SortDetails = sortDetails;
            Expression = expression;
        }

        public IColumn Column { get; }
        public ISort SortDetails { get; }
        public Expression<Func<TModel, object>> Expression { get; set; }
    }
}
