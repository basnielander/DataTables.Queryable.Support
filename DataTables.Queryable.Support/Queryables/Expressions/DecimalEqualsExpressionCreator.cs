namespace DataTables.Queryable.Support.Queryables.Expressions
{
    public class DecimalEqualsExpressionCreator : BaseEqualsExpressionCreator<decimal>
    {
        protected override decimal Convert(string value)
        {
            return System.Convert.ToDecimal(value);
        }
    }
}
