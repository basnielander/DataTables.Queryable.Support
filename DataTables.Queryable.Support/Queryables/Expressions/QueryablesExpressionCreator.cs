using DataTables.AspNet.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class QueryablesExpressionCreator<TModel>
    {
        private readonly IDataTablesRequest request;
        private readonly IEnumerable<IPropertyExpressionCreator> propertyExpressionCreators;
        private readonly ParameterExpression parameterExpression = Expression.Parameter(typeof(TModel), "item");

        public QueryablesExpressionCreator(IDataTablesRequest request, IEnumerable<IPropertyExpressionCreator> propertyExpressionCreators)
        {
            this.request = request;
            this.propertyExpressionCreators = propertyExpressionCreators;            
        }

        public QueryableExpressions<TModel> CreateExpressions()
        {
            var searchExpressions = CreateSearchExpressions();
            var columnFilterExpressions = CreateColumnFilterExpressions();
            var sortExpressions = CreateSortExpressions();

            return new QueryableExpressions<TModel>(searchExpressions, columnFilterExpressions, sortExpressions);
        }

        public IEnumerable<FilterExpression<TModel>> CreateSearchExpressions()
        {
            if (NoSearchCriteriaSpecified()) return null;

            var searchableColumns = from column in request.Columns
                                    where column.IsSearchable
                                    select column;

            if (SearchIsNotEnabledOnAnyColumn(searchableColumns)) return null;

            var columnFilterExpressions = new List<FilterExpression<TModel>>();

            foreach (var column in searchableColumns)
            {
                Expression<Func<TModel, bool>> contains = GetFilterExpression(column, request.Search.Value);

                if (contains != null)
                {
                    columnFilterExpressions.Add(new FilterExpression<TModel>(column, request.Search, contains));
                }
            }

            return columnFilterExpressions;
        }        

        public IEnumerable<FilterExpression<TModel>> CreateColumnFilterExpressions()
        {
            var columnsWithFilters = from column in request.Columns
                                     where column.Search != null && string.IsNullOrWhiteSpace(column.Search.Value) == false
                                     select column;

            if (columnsWithFilters.Count() == 0) return null;


            var columnFilterExpressions = new List<FilterExpression<TModel>>();

            foreach (var column in columnsWithFilters)
            {
                Expression<Func<TModel, bool>> contains = GetFilterExpression(column, column.Search.Value);

                columnFilterExpressions.Add(new FilterExpression<TModel>(column, column.Search, contains));
            }

            return columnFilterExpressions;
        }

        public IEnumerable<OrderExpression<TModel>> CreateSortExpressions()
        {
            var sortingColumns = from column in request.Columns
                                    where column.IsSortable && column.Sort != null 
                                    orderby column.Sort.Order
                                    select column;

            var columnSortExpressions = new List<OrderExpression<TModel>>();

            foreach (var column in sortingColumns)
            {
                Expression<Func<TModel, object>> orderBy = GetSortExpression(column, column.Sort);

                columnSortExpressions.Add(new OrderExpression<TModel>(column, column.Sort, orderBy));
            }

            return columnSortExpressions;            
        }

        private static Expression<Func<TModel, bool>> CombineColumnSearchFilters(List<Expression<Func<TModel, bool>>> columnFilterExpressions)
        {
            Expression<Func<TModel, bool>> combinedColumnSearchExpression = columnFilterExpressions.First();

            foreach (var columnFilterExpression in columnFilterExpressions.Skip(1))
            {
                var combination = Expression.OrElse(combinedColumnSearchExpression.Body, columnFilterExpression.Body);
                combinedColumnSearchExpression = Expression.Lambda<Func<TModel, bool>>(combination, combinedColumnSearchExpression.Parameters[0]);
            }

            return combinedColumnSearchExpression;
        }

        private static bool SearchIsNotEnabledOnAnyColumn(IEnumerable<IColumn> searchableColumns)
        {
            return searchableColumns.Count() == 0;
        }

        private bool NoSearchCriteriaSpecified()
        {
            return request.Search == null || string.IsNullOrWhiteSpace(request.Search.Value);
        }

        private Expression<Func<TModel, object>> GetSortExpression(IColumn column, ISort sortValue)
        {
            var sourcePropertyName = column.Field ?? column.Name;
            var sourceProperty = GetProperty<TModel>.ByName(sourcePropertyName);

            if (sourceProperty == null)
            {
                throw new DataTablesException(string.Format(CultureInfo.CurrentUICulture, "Cannot find a Get-property with the name '{0}' on type '{1}'", sourcePropertyName, typeof(TModel).FullName));
            }

            var expression = Expression.Property(parameterExpression, sourceProperty);
            return Expression.Lambda<Func<TModel, object>>(expression, new ParameterExpression[] { parameterExpression }); ;
        }

        private Expression<Func<TModel, bool>> GetFilterExpression(IColumn column, string filterValue)
        {
            var sourcePropertyName = column.Field ?? column.Name;
            var sourceProperty = GetProperty<TModel>.ByName(sourcePropertyName);

            if (sourceProperty == null)
            {
                throw new DataTablesException(string.Format(CultureInfo.CurrentUICulture, "Cannot find a Get-property with the name '{0}' on type '{1}'", sourcePropertyName, typeof(TModel).FullName));
            }

            var sourcePropertyType = sourceProperty.PropertyType;
            var sourceNullableType = Nullable.GetUnderlyingType(sourcePropertyType);

            var expressionCreator = propertyExpressionCreators.FirstOrDefault(creator => creator.TargetType.Equals(sourcePropertyType) ||
                                                                                          creator.TargetType.Equals(sourceNullableType));

            if (expressionCreator == null)
            {
                throw new DataTablesException(string.Format(CultureInfo.CurrentUICulture, "Cannot find an Expression Creator for type '{0}'", sourceProperty.PropertyType.FullName));
            }

            return expressionCreator.CreateExpression<TModel>(column, filterValue, parameterExpression);            
        }
        
    }
}
