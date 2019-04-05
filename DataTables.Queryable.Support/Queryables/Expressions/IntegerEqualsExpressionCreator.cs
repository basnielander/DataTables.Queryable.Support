using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class IntegerEqualsExpressionCreator : BaseEqualsExpressionCreator<int>
    {
        protected override int Convert(string value)
        {
            return System.Convert.ToInt32(value);
        }
    }
}
