using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions.Creators
{
    public interface IPropertyExpressionCreator
    {
        Type TargetType { get; }

        ExpressionCreatorSupport Supports { get; }

        Expression<Func<TModel, bool>> CreateExpression<TModel>(IColumn column, ISearch search, ParameterExpression parameterExpression);
    }
}
