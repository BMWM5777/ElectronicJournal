using System.Collections.Generic;

namespace ElectronicJournal.Models
{
    public class Group
    {
        public Group()
        {
            GradeList = new List<Grade>();
            UserList = new List<User>();
            SubjectList = new List<Subject>();
            DateList = new List<GradesDate>();
            SubjectGroupList = new List<SubjectGroup>();
        }
        public int Id { get; set; }
        public string GroupName { get; set; }
        public List<Grade> GradeList { get; set; }
        public List<User> UserList { get; set; }
        public List<Subject> SubjectList { get; set; }
        public List<GradesDate> DateList { get; set; }
        public List<SubjectGroup> SubjectGroupList { get; set; }    
    }
}
