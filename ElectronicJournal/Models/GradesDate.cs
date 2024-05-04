using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicJournal.Models
{
    public class GradesDate
    {
        public int Id { get; set; }
        public string Date { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        public int? GroupId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public int? SubjectId { get; set; }


    }
}
