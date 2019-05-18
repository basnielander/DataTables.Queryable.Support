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
    public class StringContainsExpressionCreatorShould
    {
        [Fact(DisplayName = "")]
        public void MyTestMethod()
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
    }
}
