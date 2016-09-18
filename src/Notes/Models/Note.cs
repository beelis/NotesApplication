using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.Models
{
    public class Note
    {
        public int ID { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public PriorityEnum PriorityEnum { get; set; }

        public Boolean Finished { get; set; }

        [Required]
        public string Title { get; set; }

        public string NoteText { get; set; }

    }
}
