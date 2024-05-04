using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicJournal.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int grade { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Groups { get; set; }
        public int? GroupId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int? SubjectId { get; set; }
    }
}
