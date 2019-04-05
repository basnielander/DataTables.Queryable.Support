using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public interface IPropertyExpressionCreator
    {
        Type TargetType { get; }

        Expression<Func<TModel, bool>> CreateExpression<TModel>(IColumn column, string filterOrSearchValue, ParameterExpression parameterExpression);
    }
}
