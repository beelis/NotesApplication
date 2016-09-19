using System;
using System.ComponentModel.DataAnnotations;

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
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy - mm:HH}")]
        public DateTime FinishDate { get; set; }

        [Required]
        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Priority")]
        [DisplayFormat(DataFormatString = "<div class=\"rating\">{0}</div>", HtmlEncode = false)]
        public PriorityEnum PriorityEnum { get; set; }

        public Boolean Finished { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string NoteText { get; set; }

    }
}
