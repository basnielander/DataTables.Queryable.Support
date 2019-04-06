using DataTables.Queryable.Support.Queryables.Expressions;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables
{
    public class QueryablesProcessor : IDataTablesRequestProcessor
    {
        private readonly IMapper mapper;

        public static readonly IList<IPropertyExpressionCreator> DefaultExpressionCreators = new List<IPropertyExpressionCreator>()
            {
                new IntegerEqualsExpressionCreator(),
                new BooleanEqualsExpressionCreator(),
                new DateTimeEqualsExpressionCreator(),
                new DecimalEqualsExpressionCreator(),
                new StringContainsExpressionCreator(),
            };

        public QueryablesProcessor(IMapper mapper)
        {
            this.mapper = mapper;

            PropertyExpressionCreators = DefaultExpressionCreators;
        }

        public QueryablesProcessor(IMapper mapper, IEnumerable<IPropertyExpressionCreator> propertyExpressionCreators) 
        {
            this.mapper = mapper;
            
            PropertyExpressionCreators = MergeCreators(propertyExpressionCreators);
        }

        private IEnumerable<IPropertyExpressionCreator> MergeCreators(IEnumerable<IPropertyExpressionCreator> newPropertyExpressionCreators)
        {
            if (newPropertyExpressionCreators == null || newPropertyExpressionCreators.Any() == false)
            {
                return DefaultExpressionCreators;
            }

            var combinedCreators = DefaultExpressionCreators.Where(creator => !newPropertyExpressionCreators.Any(newCreator => newCreator.TargetType == creator.TargetType)).ToList();

            combinedCreators.AddRange(newPropertyExpressionCreators);

            return combinedCreators;
        }

        public IEnumerable<IPropertyExpressionCreator> PropertyExpressionCreators { get; set; }

        public IDataTablesResponse CreateResponse<TDomainModel, TViewModel>(IDataTablesRequest request, Func<IOrderedQueryable<TDomainModel>> getDomainModelItemsMethod)
        {
            var expressionCreator = new QueryablesExpressionCreator<TViewModel>(request, PropertyExpressionCreators);

            var viewModelExpressions = expressionCreator.CreateExpressions();

            var queryableUnfiltered = getDomainModelItemsMethod();
            var queryableWithSearchAndColumnFiltering = ApplySearchAndColumnFilters(queryableUnfiltered, viewModelExpressions.GetCombinedSearchExpression());
            var queryableWithSearchAndColumnFilteringAndSorting = ApplySort(queryableWithSearchAndColumnFiltering, viewModelExpressions.SortExpressions);
            
            var pagedDomainModelResults = ApplyPaging(request, queryableWithSearchAndColumnFilteringAndSorting).ToList();
            var pagedViewModelResults = mapper.Map<List<TViewModel>>(pagedDomainModelResults);

            return DataTablesResponse.Create(request, queryableUnfiltered.Count(), queryableWithSearchAndColumnFiltering.Count(), pagedViewModelResults);
        }

        private IOrderedQueryable<TDomainModel> ApplySearchAndColumnFilters<TDomainModel, TViewModel>(IOrderedQueryable<TDomainModel> queryableUnfiltered, Expression<Func<TViewModel, bool>> viewModelSearchExpression)
        {
            var queryableWithSearchQuery = queryableUnfiltered;
            
            if (viewModelSearchExpression != null)
            {
                var domainModelSearchExpression = mapper.MapExpression<Expression<Func<TDomainModel, bool>>>(viewModelSearchExpression);
                queryableWithSearchQuery = queryableUnfiltered.Where(domainModelSearchExpression) as IOrderedQueryable<TDomainModel>;
            }

            return queryableWithSearchQuery;
        }

        private IQueryable<TDomainModel> ApplySort<TDomainModel, TViewModel>(IOrderedQueryable<TDomainModel> queryableWithSearchQuery, IEnumerable<OrderExpression<TViewModel>> viewModelSortExpressions)
        {
            if (viewModelSortExpressions == null || viewModelSortExpressions.Count() == 0) return queryableWithSearchQuery;

            var firstViewModelSortExpression = viewModelSortExpressions.First();
            var domainModelSortExpression = mapper.MapExpression<Expression<Func<TDomainModel, object>>>(firstViewModelSortExpression.Expression);

            if (firstViewModelSortExpression.SortDetails.Direction == SortDirection.Ascending)
            {
                queryableWithSearchQuery = queryableWithSearchQuery.OrderBy(domainModelSortExpression) as IOrderedQueryable<TDomainModel>;
            }
            else
            {
                queryableWithSearchQuery = queryableWithSearchQuery.OrderByDescending(domainModelSortExpression) as IOrderedQueryable<TDomainModel>;
            }

            foreach (var viewModelSortExpression in viewModelSortExpressions.Skip(1))
            {
                domainModelSortExpression = mapper.MapExpression<Expression<Func<TDomainModel, object>>>(viewModelSortExpression.Expression);

                if (viewModelSortExpression.SortDetails.Direction == SortDirection.Ascending)
                {
                    queryableWithSearchQuery = queryableWithSearchQuery.ThenBy(domainModelSortExpression) as IOrderedQueryable<TDomainModel>;
                }
                else
                {
                    queryableWithSearchQuery = queryableWithSearchQuery.ThenByDescending(domainModelSortExpression) as IOrderedQueryable<TDomainModel>;
                }
            }

            return queryableWithSearchQuery;
        }

        private IQueryable<TModel> ApplyPaging<TModel>(IDataTablesRequest request, IQueryable<TModel> queryable)
        {
            var queryableWithPaging = queryable.Skip(request.Start);

            if (request.Length > 0)
            {
                queryableWithPaging = queryableWithPaging.Take(request.Length);
            }

            return queryableWithPaging;
        }

    }
}
