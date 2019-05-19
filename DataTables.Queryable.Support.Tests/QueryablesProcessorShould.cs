using AutoMapper;
using DataTables.AspNet.Core;
using DataTables.Queryable.Support.Queryables;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DataTables.Queryable.Support.Tests
{
    public class QueryablesProcessorShould
    {
        // This test doesn't work, because the DataTables functionality requires a Http context
        //[Fact(DisplayName = "CreateResponse, request for an item that contains a 'b' should return a response with 1 item.")]
        public void CreateResponseRequestForItemThatContainsAbShouldReturnAResponseWith1Item()
        {
            // arrange
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TestModel, TestViewModel>();
            });            
            IMapper mapper = new Mapper(config);
            

            var processor = new QueryablesProcessor(mapper);

            var testItemToBeFound = new TestModel() { IntProp = 1, StringProp = "abc" };
            var testItemNotToBeFound = new TestModel() { IntProp = 2, StringProp = "cde" };

            var testList = new List<TestModel>()
            {
                testItemToBeFound,
                testItemNotToBeFound
            };

            Func<IQueryable<TestModel>> getTestItems = () => testList.AsQueryable();

            var request = InitTestModelDataTablesRequest();
            var search = new Mock<ISearch>();
            search.Setup(srch => srch.IsRegex).Returns(false);
            search.Setup(srch => srch.Value).Returns("b");
            request.Setup(req => req.Search).Returns(search.Object);

            // act
            IDataTablesResponse response = processor.CreateResponse<TestModel, TestViewModel>(request.Object, getTestItems);

            // assert
            Assert.Equal(2, response.TotalRecords);
            Assert.Equal(1, response.TotalRecordsFiltered);
            Assert.Single(response.Data as IEnumerable<TestModel>);
        }

        private static Mock<IDataTablesRequest> InitTestModelDataTablesRequest()
        {
            var column1 = new Mock<IColumn>();
            var column2 = new Mock<IColumn>();
            
            var request = new Mock<IDataTablesRequest>();
            request.Setup(req => req.Columns).Returns(new List<IColumn>() { column1.Object, column2.Object });            

            return request;
        }
    }
}
