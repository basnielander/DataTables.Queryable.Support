using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions.Creators
{
    public class StringContainsExpressionCreator : BaseExpressionCreator<string>
    {        
        public override ExpressionCreatorSupport Supports => ExpressionCreatorSupport.Search | ExpressionCreatorSupport.ColumnFilter;

        public override bool SupportsReqularExpressions => false;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="column"></param>
        /// <param name="search"></param>
        /// <param name="parameterExpression"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the searchquery is based on an regular expression</exception>
        public override Expression<Func<TModel, bool>> GetExpression<TModel>(IColumn column, ISearch search, ParameterExpression parameterExpression) 
        {            
            var sourcePropertyName = column.Field ?? column.Name;
            var sourceProperty = GetProperty<TModel>.ByName(sourcePropertyName);

            var logicalMethod = TargetType.GetMethod(nameof(string.Contains), new[] { typeof(string), typeof(StringComparison) });
            var expression = Expression.Call(Expression.Property(parameterExpression, sourceProperty), logicalMethod, Expression.Constant(search.Value), Expression.Constant(StringComparison.CurrentCultureIgnoreCase));

            return Expression.Lambda<Func<TModel, bool>>(expression, new ParameterExpression[] { parameterExpression });
        }        
    }
}
