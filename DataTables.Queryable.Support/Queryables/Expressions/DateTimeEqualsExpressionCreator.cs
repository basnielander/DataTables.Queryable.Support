using DataTables.AspNet.Core;
using System;
using System.Linq.Expressions;

namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class DateTimeEqualsExpressionCreator : BaseEqualsExpressionCreator<DateTime>
    {
        protected override DateTime Convert(string value)
        {
            return System.Convert.ToDateTime(value);
        }
    }
}
