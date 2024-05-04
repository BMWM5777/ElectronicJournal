using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicJournal.Models
{
    public class User
    {
        public User()
        {
            GradeList = new List<Grade>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [ForeignKey("Roles")]
        public int RoleId { get; set; }
        public Role Roles { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Groups { get; set; }
        public int? GroupId { get; set; }
        public List<Grade> GradeList { get; set; }
    }
}
