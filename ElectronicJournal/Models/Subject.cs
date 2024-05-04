    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicJournal.Models
{
    public class Subject
    {
        public Subject()
        {
            GradeList = new List<Grade>();
            subjectGroups = new List<SubjectGroup>();
        }
        public int Id { get; set; }
        public string SubjectName { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        public List<Grade> GradeList { get; set; }
        public List<SubjectGroup> subjectGroups { get; set; }   
    }
}
