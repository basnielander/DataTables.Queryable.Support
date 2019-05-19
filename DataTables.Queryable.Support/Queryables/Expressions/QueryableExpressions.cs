using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class QueryableExpressions<TModel>
    {
        public QueryableExpressions(IEnumerable<FilterExpression<TModel>> searchExpressions, IEnumerable<FilterExpression<TModel>> columnFilterExpressions, IEnumerable<OrderExpression<TModel>> sortExpressions)
        {
            SearchExpressions = (searchExpressions?.ToList() ?? new List<FilterExpression<TModel>>()).AsReadOnly();
            ColumnFilterExpressions = (columnFilterExpressions?.ToList() ?? new List<FilterExpression<TModel>>()).AsReadOnly();
            SortExpressions = (sortExpressions?.ToList() ?? new List<OrderExpression<TModel>>()).AsReadOnly();
        }

        public ReadOnlyCollection<FilterExpression<TModel>> SearchExpressions { get; }

        public ReadOnlyCollection<FilterExpression<TModel>> ColumnFilterExpressions { get; }

        public ReadOnlyCollection<OrderExpression<TModel>> SortExpressions { get; }

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
