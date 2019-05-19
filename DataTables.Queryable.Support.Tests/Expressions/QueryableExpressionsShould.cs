using DataTables.AspNet.Core;
using DataTables.Queryable.Support.Queryables.Expressions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        [Theory(DisplayName = "GetCombinedSearchExpression() should return Expression 1 || Expression 2 when the 2 expressions are passed as search expressions.")]
        [InlineData(false, false, 0)]
        [InlineData(true, false, 1)]
        [InlineData(false, true, 1)]
        [InlineData(true, true, 1)]
        public void GetCombinedSearchExpressionShouldReturnTheOrElseResultOfTwoSearchExpressions(bool expression1Result, bool expression2Result, int expectedResultCount)
        {
            // arrange
            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(false);
            
            var parameterExpression = Expression.Parameter(typeof(TestModel));

            var expression1 = Expression.Lambda<Func<TestModel, bool>>(Expression.Constant(expression1Result), parameterExpression);
            var expression2 = Expression.Lambda<Func<TestModel, bool>>(Expression.Constant(expression2Result), parameterExpression);

            var testSearchExpression1 = new FilterExpression<TestModel>(columnMock.Object, searchMock.Object, expression1);
            var testSearchExpression2 = new FilterExpression<TestModel>(columnMock.Object, searchMock.Object, expression2);
            var searchExpressions = new List<FilterExpression<TestModel>>() { testSearchExpression1, testSearchExpression2 };

            var queryableExpression = new QueryableExpressions<TestModel>(searchExpressions, null, null);

            var testList = new List<TestModel>()
            {
                new TestModel() { IntProp = 1 }
            };

            // act
            var resultingExpression = queryableExpression.GetCombinedSearchExpression();

            var actualResults = testList.Where(resultingExpression.Compile());

            // assert
            Assert.Equal(expectedResultCount, actualResults.Count());
        }

        [Theory(DisplayName = "GetCombinedSearchExpression() should return Expression 1 && Expression 2 when the 2 expressions are passed as column filter expressions.")]
        [InlineData(false, false, 0)]
        [InlineData(true, false, 0)]
        [InlineData(false, true, 0)]
        [InlineData(true, true, 1)]
        public void GetCombinedSearchExpressionShouldReturnTheAndAlsoResultOfTwoColumnFilterExpressions(bool expression1Result, bool expression2Result, int expectedResultCount)
        {
            // arrange
            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(false);

            var parameterExpression = Expression.Parameter(typeof(TestModel));

            var expression1 = Expression.Lambda<Func<TestModel, bool>>(Expression.Constant(expression1Result), parameterExpression);
            var expression2 = Expression.Lambda<Func<TestModel, bool>>(Expression.Constant(expression2Result), parameterExpression);

            var testColumnFilterExpression1 = new FilterExpression<TestModel>(columnMock.Object, searchMock.Object, expression1);
            var testColumnFilterExpression2 = new FilterExpression<TestModel>(columnMock.Object, searchMock.Object, expression2);
            var columnFilterExpressions = new List<FilterExpression<TestModel>>() { testColumnFilterExpression1, testColumnFilterExpression2 };

            var queryableExpression = new QueryableExpressions<TestModel>(null, columnFilterExpressions, null);

            var testList = new List<TestModel>()
            {
                new TestModel() { IntProp = 1 }
            };

            // act
            var resultingExpression = queryableExpression.GetCombinedSearchExpression();

            var actualResults = testList.Where(resultingExpression.Compile());

            // assert
            Assert.Equal(expectedResultCount, actualResults.Count());
        }

        [Theory(DisplayName = "GetCombinedSearchExpression() should return Expression 1 && Expression 2 when 1 column filter expressions and 1 search expression are passed as constructor parameters.")]
        [InlineData(false, false, 0)]
        [InlineData(true, false, 0)]
        [InlineData(false, true, 0)]
        [InlineData(true, true, 1)]
        public void GetCombinedSearchExpressionShouldReturnTheAndAlsoResultOfAColumnFilterExpressionsAndASearchExpression(bool expression1Result, bool expression2Result, int expectedResultCount)
        {
            // arrange
            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(false);

            var parameterExpression = Expression.Parameter(typeof(TestModel));

            var expression1 = Expression.Lambda<Func<TestModel, bool>>(Expression.Constant(expression1Result), parameterExpression);
            var expression2 = Expression.Lambda<Func<TestModel, bool>>(Expression.Constant(expression2Result), parameterExpression);

            var testColumnFilterExpression = new FilterExpression<TestModel>(columnMock.Object, searchMock.Object, expression1);            
            var columnFilterExpressions = new List<FilterExpression<TestModel>>() { testColumnFilterExpression };

            var testSearchExpression = new FilterExpression<TestModel>(columnMock.Object, searchMock.Object, expression2);
            var searchExpressions = new List<FilterExpression<TestModel>>() { testSearchExpression };

            var queryableExpression = new QueryableExpressions<TestModel>(searchExpressions, columnFilterExpressions, null);

            var testList = new List<TestModel>()
            {
                new TestModel() { IntProp = 1 }
            };

            // act
            var resultingExpression = queryableExpression.GetCombinedSearchExpression();

            var actualResults = testList.Where(resultingExpression.Compile());

            // assert
            Assert.Equal(expectedResultCount, actualResults.Count());
        }

        [Fact(DisplayName = "GetCombinedSearchExpression() should return items ordered by the given expression.")]
        public void GetCombinedSearchExpressionShouldReturnItemsOrderedByTheGivenExpression()
        {
            // arrange
            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var sortMock = new Mock<ISort>();
            sortMock.SetupGet(order => order.Direction).Returns(SortDirection.Ascending);

            var parameterExpression = Expression.Parameter(typeof(TestModel));

            var expression1 = Expression.Lambda<Func<TestModel, object>>(Expression.Convert(Expression.Property(parameterExpression, nameof(TestModel.IntProp)), typeof(object)), parameterExpression);

            var testColumnFilterExpression = new OrderExpression<TestModel>(columnMock.Object, sortMock.Object, expression1);
            var orderExpressions = new List<OrderExpression<TestModel>>() { testColumnFilterExpression };

            var queryableExpression = new QueryableExpressions<TestModel>(null, null, orderExpressions);

            var testList = new List<TestModel>()
            {
                new TestModel() { IntProp = 3 },
                new TestModel() { IntProp = 1 },
                new TestModel() { IntProp = 2 }
            };

            // act
            var resultingExpression = queryableExpression.SortExpressions.First();

            var actualResults = testList.OrderBy(resultingExpression.Expression.Compile());

            // assert
            Assert.Collection(actualResults,
                item => Assert.Equal(1, item.IntProp),
                item => Assert.Equal(2, item.IntProp),
                item => Assert.Equal(3, item.IntProp));
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
