using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicJournal.Models
{
    public class SubjectGroup
    {
        public int Id { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        public int? GroupId { get; set; }
        [ForeignKey("SubjectNameId")]
        public virtual Subject Subjects { get; set; }
        public int? SubjectNameId { get; set; }  
    }
}
