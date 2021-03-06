﻿using DataTables.AspNet.Core;
using DataTables.Queryable.Support.Queryables.Expressions.Creators;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace DataTables.Queryable.Support.Tests.Expressions.Creators
{
    public class StringContainsExpressionCreatorShould
    {
        [Fact(DisplayName = "The String.Contains() creator should return an expression with which a List<> can be succesfully queried")]
        public void CreatorShouldReturnAnExpressionThatCorrectlyFiltersAListUsingContainsMethod()
        {
            // arrange
            var creator = new StringContainsExpressionCreator();

            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.StringProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(false);
            searchMock.SetupGet(search => search.Value).Returns("Test");

            var parameterExpression = Expression.Parameter(typeof(TestModel));

            var testItemToBeFound = new TestModel() { StringProp = "This is a test." };
            var testItemNotToBeFound = new TestModel() { StringProp = "And this one shouldn't be found." };

            var testList = new List<TestModel>()
            {
                testItemToBeFound,
                testItemNotToBeFound
            };

            // act
            var searchExpression = creator.CreateExpression<TestModel>(columnMock.Object, searchMock.Object, parameterExpression);
            var searchResults = testList.Where(searchExpression.Compile());

            // assert
            Assert.Collection(searchResults, searchHit => Assert.Equal(testItemToBeFound, searchHit));
        }

        [Fact(DisplayName = "The String.Contains() creator should throw a NotSupportedException when the searchquery is based on a regular expression")]
        public void CreatorShouldThrowNotSupportedExceptionWhenTheSearchQueryIsBasedOnARegularExpression()
        {
            // arrange
            var creator = new StringContainsExpressionCreator();

            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.StringProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(true);

            var parameterExpression = Expression.Parameter(typeof(TestModel));            

            // act & assert
            Assert.Throws<NotSupportedException>(() => creator.CreateExpression<TestModel>(columnMock.Object, searchMock.Object, parameterExpression));
        }

        [Fact(DisplayName = "The String.Contains() creator should throw a ArgumentOutOfRangeException when the source field is not a property of the model")]
        public void CreatorShouldThrowAnArgumentOutOfRangeExceptionWhenTheSourceFieldIsNotAPropertyOfTheModel()
        {
            // arrange
            var creator = new StringContainsExpressionCreator();

            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns("UnknownField");

            var searchMock = new Mock<ISearch>();
            
            var parameterExpression = Expression.Parameter(typeof(TestModel));

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => creator.CreateExpression<TestModel>(columnMock.Object, searchMock.Object, parameterExpression));
        }
    }
}
