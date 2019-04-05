using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Datatables.Samples.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }
    }
}
