using System.Collections;
using System.Collections.Generic;

namespace ElectronicJournal.Models
{
    public class AllModels
    {
        public IEnumerable<Grade> GradesList { get; set; }
        public IEnumerable<Group> GroupsList { get; set; }
        public IEnumerable<Role> RolesList { get; set; }
        public IEnumerable<Subject> SubjectsList { get; set; }
        public IEnumerable<User> UsersList { get; set; }
        public IEnumerable<GradesDate> DateList { get; set; }
        public IEnumerable<SubjectGroup> SubjectGroupsList { get; set; }
    }
}
