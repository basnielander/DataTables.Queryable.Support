using System;
using System.ComponentModel.DataAnnotations;

namespace Datatables.Samples.Models
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        public int CreatedById { get; set; }
        
        public Person CreatedBy { get; set; }

        public int? EstimatedDuration { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal? ExpectedCosts { get; set; }

        public bool Finished { get; set; }
    }
}
