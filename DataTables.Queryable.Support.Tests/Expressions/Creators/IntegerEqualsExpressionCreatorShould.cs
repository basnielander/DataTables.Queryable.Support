using DataTables.AspNet.Core;
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
    public class IntegerEqualsExpressionCreatorShould
    {
        [Fact(DisplayName = "The int.Equals() creator should return an expression with which a List<> can be succesfully queried")]
        public void CreatorShouldReturnAnExpressionThatCorrectlyFiltersAListUsingEqualsMethod()
        {
            // arrange
            var creator = new IntegerEqualsExpressionCreator();

            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(false);
            searchMock.SetupGet(search => search.Value).Returns("1");

            var parameterExpression = Expression.Parameter(typeof(TestModel));

            var testItemToBeFound = new TestModel() { IntProp = 1 };
            var testItemNotToBeFound = new TestModel() { IntProp = 2 };

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

        [Fact(DisplayName = "The int.Equals() creator should return null when the searchquery does not contain an integer value")]
        public void CreatorShouldReturnNoItemsWhenTheQueryDoesNotContainAnIntegerValue()
        {
            // arrange
            var creator = new IntegerEqualsExpressionCreator();

            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(false);
            searchMock.SetupGet(search => search.Value).Returns("One");

            var parameterExpression = Expression.Parameter(typeof(TestModel));
                        
            // act
            var searchExpression = creator.CreateExpression<TestModel>(columnMock.Object, searchMock.Object, parameterExpression);
            
            // assert
            Assert.Null(searchExpression);
        }

        [Fact(DisplayName = "The int.Equals() creator should throw a NotSupportedException when the searchquery is based on a regular expression")]
        public void CreatorShouldThrowNotSupportedExceptionWhenTheSearchQueryIsBasedOnARegularExpression()
        {
            // arrange
            var creator = new IntegerEqualsExpressionCreator();

            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns(nameof(TestModel.IntProp));

            var searchMock = new Mock<ISearch>();
            searchMock.SetupGet(search => search.IsRegex).Returns(true);

            var parameterExpression = Expression.Parameter(typeof(TestModel));            

            // act & assert
            Assert.Throws<NotSupportedException>(() => creator.CreateExpression<TestModel>(columnMock.Object, searchMock.Object, parameterExpression));
        }

        [Fact(DisplayName = "The int.Equals() creator should throw a ArgumentOutOfRangeException when the source field is not a property of the model")]
        public void CreatorShouldThrowAnArgumentOutOfRangeExceptionWhenTheSourceFieldIsNotAPropertyOfTheModel()
        {
            // arrange
            var creator = new IntegerEqualsExpressionCreator();

            var columnMock = new Mock<IColumn>();
            columnMock.SetupGet(model => model.Field).Returns("UnknownField");

            var searchMock = new Mock<ISearch>();
            
            var parameterExpression = Expression.Parameter(typeof(TestModel));

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => creator.CreateExpression<TestModel>(columnMock.Object, searchMock.Object, parameterExpression));
        }
    }
}
