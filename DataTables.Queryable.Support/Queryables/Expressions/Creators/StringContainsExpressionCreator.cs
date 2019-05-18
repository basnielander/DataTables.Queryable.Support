using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions.Creators
{
    public class StringContainsExpressionCreator : IPropertyExpressionCreator
    {
        public Type TargetType => typeof(string);

        public ExpressionCreatorSupport Supports => ExpressionCreatorSupport.Search | ExpressionCreatorSupport.ColumnFilter;

        public Expression<Func<TModel, bool>> CreateExpression<TModel>(IColumn column, ISearch search, ParameterExpression parameterExpression) 
        {
            if (search.IsRegex)
            {
                throw new NotSupportedException($"The expression creator {nameof(StringContainsExpressionCreator)} does not support regular expressions.");
            }

            var sourcePropertyName = column.Field ?? column.Name;
            var sourceProperty = GetProperty<TModel>.ByName(sourcePropertyName);

            var logicalMethod = TargetType.GetMethod(nameof(string.Contains), new[] { typeof(string), typeof(StringComparison) });
            var expression = Expression.Call(Expression.Property(parameterExpression, sourceProperty), logicalMethod, Expression.Constant(search.Value), Expression.Constant(StringComparison.CurrentCultureIgnoreCase));

            return Expression.Lambda<Func<TModel, bool>>(expression, new ParameterExpression[] { parameterExpression });
        }        
    }
}
