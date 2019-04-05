using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DataTables.Queryable.Support
{
    public class DataTablesException : Exception
    {
        public DataTablesException()
        {
        }

        public DataTablesException(string message) : base(message)
        {
        }

        public DataTablesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataTablesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
