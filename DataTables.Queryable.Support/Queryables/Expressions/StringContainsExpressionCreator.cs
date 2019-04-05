using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class StringContainsExpressionCreator : IPropertyExpressionCreator
    {
        public Type TargetType => typeof(string);

        public Expression<Func<TModel, bool>> CreateExpression<TModel>(IColumn column, string filterOrSearchValue, ParameterExpression parameterExpression) 
        {
            var sourcePropertyName = column.Field ?? column.Name;
            var sourceProperty = GetProperty<TModel>.ByName(sourcePropertyName);

            var logicalMethod = TargetType.GetMethod(nameof(string.Contains), new[] { typeof(string) });
            var expression = Expression.Call(Expression.Property(parameterExpression, sourceProperty), logicalMethod, Expression.Constant(filterOrSearchValue));

            return Expression.Lambda<Func<TModel, bool>>(expression, new ParameterExpression[] { parameterExpression });
        }        
    }
}
