//using DataTables.Support;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;
//using Xunit;
//using System.Linq.Expressions;
//using DataTables.AspNet.Core;
//using Moq;
//using DataTables.AspNet.AspNetCore;

//namespace DataTables.Tests.DataTablesQueryable
//{
//    public class DataTablesExtensionShould
//    {
//        private Mock<IDataTablesRequest> _requestMock = new Mock<IDataTablesRequest>();
//        private Mock<IColumn> _productNameColumnMock = new Mock<IColumn>();
//        private Mock<IColumn> _quantityColumnMock = new Mock<IColumn>();

//        public DataTablesExtensionShould()
//        {                        
//            _productNameColumnMock.SetupGet(column => column.Field).Returns("ProductName");
//            _productNameColumnMock.SetupGet(column => column.IsSearchable).Returns(true);
//            _productNameColumnMock.SetupGet(column => column.IsSortable).Returns(true);
//            _quantityColumnMock.SetupGet(column => column.Field).Returns("Quantity");
//            _quantityColumnMock.SetupGet(column => column.IsSearchable).Returns(true);
//            _quantityColumnMock.SetupGet(column => column.IsSortable).Returns(false);

//            _requestMock.SetupGet(request => request.Columns).Returns(() => new List<IColumn>() { _productNameColumnMock.Object, _quantityColumnMock.Object });
//            _requestMock.SetupGet(request => request.Draw).Returns(1);
//        }

//        private void FilterProductNameColumnBy(string filterValue)
//        {
//            var columnFilterMock = new Mock<ISearch>();
//            columnFilterMock.SetupGet(filter => filter.Value).Returns(filterValue);

//            _productNameColumnMock.SetupGet(column => column.Search).Returns(columnFilterMock.Object);
//        }

//        [Fact]
//        public void FilterProductNameColumnByContainsOReturnsFilteredCountOf2()
//        {
//            // arrange
//            var totalsQueryable = Mother.GetOrderLines().AsQueryable();

//            FilterProductNameColumnBy("o");
            
//            // act
//            var response = _requestMock.Object.GetResponse<TestOrderLine>(totalsQueryable);

//            // assert
//            Assert.IsAssignableFrom<IQueryable<TestOrderLine>>(response.Data);
//            Assert.Equal(4, response.TotalRecords);
//            Assert.Equal(2, response.TotalRecordsFiltered);
//        }

//        [Fact]
//        public void PageOrderLinesWithPageSize2ShouldResultIn2Items()
//        {
//            // arrange
//            var orderLinesData = Mother.GetOrderLines().AsQueryable();

//            Paging paging = new Paging(2, 0);

//            // act
//            var response = _requestMock.Object.GetResponse<TestOrderLine>(orderLinesData, paging);

//            // assert
//            Assert.Equal(2, (response.Data as IEnumerable<TestOrderLine>).Count());
//        }

//        [Fact]
//        public void FilterProductNameColumnAndApplyPageSize1AndSkip1ContainsSpoonAsResult()
//        {
//            // arrange
//            var orderLinesData = Mother.GetOrderLines().AsQueryable();

//            FilterProductNameColumnBy("o");

//            Paging paging = new Paging(1, 1);

//            // act
//            var response = _requestMock.Object.GetResponse<TestOrderLine>(orderLinesData, paging);

//            // assert
//            Assert.Single(response.Data as IQueryable);
//        }

//        [Fact]
//        public void SearchForItemsWithAQuantityOf16ShouldReturn3Items()
//        {
//            // arrange
//            var orderLinesData = Mother.GetOrderLines().AsQueryable();

//            var searchMock = new Mock<ISearch>();
//            searchMock.SetupGet(search => search.Value).Returns("16");

//            _requestMock.SetupGet(request => request.Search).Returns(searchMock.Object);

//            // act
//            var response = _requestMock.Object.GetResponse<TestOrderLine>(orderLinesData);

//            // assert
//            Assert.Equal(3, (response.Data as IEnumerable<TestOrderLine>).Count());
//        }
//    }
//}
