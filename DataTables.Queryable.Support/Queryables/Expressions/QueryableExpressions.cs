using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class QueryableExpressions<TModel>
    {
        public QueryableExpressions(IEnumerable<FilterExpression<TModel>> searchExpressions, IEnumerable<FilterExpression<TModel>> columnFilterExpressions, IEnumerable<OrderExpression<TModel>> sortExpressions)
        {
            SearchExpressions = searchExpressions ?? new List<FilterExpression<TModel>>();
            ColumnFilterExpressions = columnFilterExpressions ?? new List<FilterExpression<TModel>>();
            SortExpressions = sortExpressions ?? new List<OrderExpression<TModel>>();
        }

        public IEnumerable<FilterExpression<TModel>> SearchExpressions { get; }

        public IEnumerable<FilterExpression<TModel>> ColumnFilterExpressions { get; }

        public IEnumerable<OrderExpression<TModel>> SortExpressions { get; }

        public Expression<Func<TModel, bool>> GetCombinedSearchExpression()
        {
            Expression<Func<TModel, bool>> resultingExpression = null;

            foreach (var searchExpression in SearchExpressions)
            {
                if (resultingExpression == null)
                {
                    resultingExpression = searchExpression.Expression;
                }
                else
                {
                    var combination = Expression.OrElse(resultingExpression.Body, searchExpression.Expression.Body);
                    resultingExpression = Expression.Lambda<Func<TModel, bool>>(combination, resultingExpression.Parameters[0]);
                }
            }

            foreach (var searchExpression in ColumnFilterExpressions)
            {
                if (resultingExpression == null)
                {
                    resultingExpression = searchExpression.Expression;
                }
                else
                {
                    var combination = Expression.AndAlso(resultingExpression.Body, searchExpression.Expression.Body);
                    resultingExpression = Expression.Lambda<Func<TModel, bool>>(combination, resultingExpression.Parameters[0]);
                }
            }

            return resultingExpression;
        }

    }
}
