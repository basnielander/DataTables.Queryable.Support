using Datatables.Samples.Models;
using Microsoft.EntityFrameworkCore;

namespace DataTables.Sample.Data.Models
{
    public class DataTablesContext : DbContext
    {
        public DataTablesContext(DbContextOptions<DataTablesContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
    }
}
