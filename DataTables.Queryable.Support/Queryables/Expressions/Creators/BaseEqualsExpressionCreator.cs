using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTables.Queryable.Support.Queryables.Expressions.Creators
{
    public abstract class BaseEqualsExpressionCreator<TColumn> : IPropertyExpressionCreator
    {
        public Type TargetType => typeof(TColumn);

        public ExpressionCreatorSupport Supports => ExpressionCreatorSupport.Search | ExpressionCreatorSupport.ColumnFilter;

        public virtual Expression<Func<TModel, bool>> CreateExpression<TModel>(IColumn column, ISearch search, ParameterExpression parameterExpression)
        {
            if (search.IsRegex)
            {
                throw new NotSupportedException($"The expression creator {nameof(StringContainsExpressionCreator)} does not support regular expressions.");
            }

            var sourcePropertyName = column.Field ?? column.Name;
            var sourceProperty = GetProperty<TModel>.ByName(sourcePropertyName);
            var sourcePropertyType = sourceProperty.PropertyType;
            var sourceNullableType = Nullable.GetUnderlyingType(sourcePropertyType);

            Expression property = Expression.Property(parameterExpression, sourceProperty);
            bool canBeConvertedToColumnType = TryConvert(search.Value, out TColumn filterValue);

            if (canBeConvertedToColumnType)
            {
                if (TargetType.Equals(sourcePropertyType))
                {
                    Expression valueCheck = Expression.Equal(property, Expression.Constant(filterValue));

                    return Expression.Lambda<Func<TModel, bool>>(valueCheck, new ParameterExpression[] { parameterExpression });
                }
                else if (TargetType.Equals(sourceNullableType))
                {
                    Expression nullCheck = Expression.NotEqual(property, Expression.Constant(null, sourcePropertyType));
                    Expression valueCheck = Expression.Equal(property, Expression.Convert(Expression.Constant(filterValue), sourcePropertyType));

                    //Expression.Lambda
                    return Expression.Lambda<Func<TModel, bool>>(Expression.AndAlso(nullCheck, valueCheck), new ParameterExpression[] { parameterExpression });
                }
            }
            return null;
        }

        protected bool TryConvert(string value, out TColumn @out) 
        {
            bool convertSuccessfull = true;

            try
            {
                @out = Convert(value);                
            }
            catch (Exception exc)
            {
                @out = default(TColumn);
                convertSuccessfull = false;
            }

            return convertSuccessfull;
        }

        protected virtual TColumn Convert(string value)
        {
            var parseMethod = typeof(TColumn).GetMethod("Parse", new[] { typeof(string) });

            return (TColumn)parseMethod.Invoke(null, new[] { value });
        }
    }
}
