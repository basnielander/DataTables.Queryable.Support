using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions.Creators
{
    public abstract class BaseExpressionCreator<TColumn> : IPropertyExpressionCreator
    {
        public Type TargetType => typeof(TColumn);

        public abstract ExpressionCreatorSupport Supports { get; }

        public abstract bool SupportsReqularExpressions { get; }

        /// <exception cref="NotSupportedException">Thrown when the searchquery is based on an regular expression</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the column refers to a non-existing model property</exception>
        public Expression<Func<TModel, bool>> CreateExpression<TModel>(IColumn column, ISearch search, ParameterExpression parameterExpression)
        {
            ValidateCreateExpressionCall<TModel>(column, search, parameterExpression);

            return GetExpression<TModel>(column, search, parameterExpression);
        }

        public virtual void ValidateCreateExpressionCall<TModel>(IColumn column, ISearch search, ParameterExpression parameterExpression)
        {
            if (search.IsRegex && SupportsReqularExpressions == false)
            {
                throw new NotSupportedException($"The expression creator {this.GetType().FullName} does not support regular expressions.");
            }

            var sourcePropertyName = column.Field ?? column.Name;
            var sourceProperty = GetProperty<TModel>.ByName(sourcePropertyName);

            if (sourceProperty == null) throw new ArgumentOutOfRangeException(nameof(column), $"The column '{sourcePropertyName}' is not a property of the model '{typeof(TModel).FullName}'");
        }

        public abstract Expression<Func<TModel, bool>> GetExpression<TModel>(IColumn column, ISearch search, ParameterExpression parameterExpression);
    }
}
