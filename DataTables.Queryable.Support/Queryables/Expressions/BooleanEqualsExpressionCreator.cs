namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class BooleanEqualsExpressionCreator : BaseEqualsExpressionCreator<bool>
    {
        protected override bool Convert(string value)
        {
            return System.Convert.ToBoolean(value);
        }
    }
}
