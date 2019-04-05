using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Datatables.Sample.Domain.Models
{
    public class ToDoModel
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        [Required]
        public UserModel CreatedBy { get; set; }

        public int? EstimatedDuration { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal? ExpectedCosts { get; set; }

        public bool Finished { get; set; }
    }
}
