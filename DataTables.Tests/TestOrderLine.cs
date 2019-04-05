using System;
using System.Collections.Generic;
using System.Text;

namespace DataTables.Tests
{
    public class TestOrderLine
    {
        public TestOrderLine()
        {
        }

        public TestOrderLine(string productName, int quantity)
        {
            ProductName = productName;
            Quantity = quantity;
        }

        public string ProductName { get; set; }

        public int Quantity { get; set; }
    }
}
