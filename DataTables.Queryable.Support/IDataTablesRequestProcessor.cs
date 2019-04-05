using DataTables.AspNet.Core;
using System;
using System.Linq;

namespace DataTables.Queryable.Support
{
    public interface IDataTablesRequestProcessor
    {
        IDataTablesResponse CreateResponse<TSourceModel, TTargetModel>(IDataTablesRequest request, Func<IOrderedQueryable<TSourceModel>> getItemsMethod);
    }
}
