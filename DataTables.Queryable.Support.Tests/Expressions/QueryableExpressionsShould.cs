using DataTables.AspNet.Core;
using DataTables.Queryable.Support.Queryables.Expressions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace DataTables.Queryable.Support.Tests.Expressions
{
    public class QueryableExpressionsShould
    {
        [Fact(DisplayName = "GetCombinedSearchExpression() should return the single search expression if that search expression is the only expression passed as a constructor parameter.")]
        public void GetCombinedSearchExpressionShouldReturnTheSingleSearchExpressionIfOnlyThatExpressionIsEnteredToTheConstructor()
        {
            // arrange
            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(false);
            searchMock.SetupGet(search => search.Value).Returns("1");

            var parameterExpression = Expression.Parameter(typeof(TestModel));

            var expression = Expression.Lambda<Func<TestModel, bool>>(Expression.Constant(true), parameterExpression);

            var testSearchExpression = new FilterExpression<TestModel>(columnMock.Object, searchMock.Object, expression);
            var searchExpressions = new List<FilterExpression<TestModel>>() { testSearchExpression };

            var queryableExpression = new QueryableExpressions<TestModel>(searchExpressions, null, null);

            // act
            var resultingExpression = queryableExpression.GetCombinedSearchExpression();

            // assert
            Assert.Equal(testSearchExpression.Expression, resultingExpression);
        }

        [Fact(DisplayName = "GetCombinedSearchExpression() should return null if all constructor parameters are null")]
        public void GetCombinedSearchExpressionShouldReturnNullIfAllConstructorParametersAreNull()
        {
            // arrange
            var queryableExpression = new QueryableExpressions<TestModel>(null, null, null);

            // act
            var resultingExpression = queryableExpression.GetCombinedSearchExpression();

            // assert
            Assert.Null(resultingExpression);
        }
    }
}
