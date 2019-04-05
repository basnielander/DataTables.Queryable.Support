using System;

namespace Datatables.Sample.Web.Models
{
    public class ToDoViewModel
    {        
        public int Id { get; set; }
        
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public int? EstimatedDuration { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal? ExpectedCosts { get; set; }

        public bool Finished { get; set; }
    }
}
