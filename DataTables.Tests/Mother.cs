using System.Collections.Generic;

namespace DataTables.Tests
{
    public static class Mother
    {
        public static List<TestOrderLine> GetOrderLines()
        {
            return new List<TestOrderLine>()
            {
                new TestOrderLine("Plate", 8),
                new TestOrderLine("Knife", 16),
                new TestOrderLine("Fork", 16),
                new TestOrderLine("Spoon", 16),
            };
        }
    }
}
