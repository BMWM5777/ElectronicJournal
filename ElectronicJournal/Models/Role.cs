using System.Collections.Generic;

namespace ElectronicJournal.Models
{
    public class Role
    {
        public Role()
        {
            UserList = new List<User>();
        }
        public int Id { get; set; }
        public string RoleName { get; set; }
        public List<User> UserList { get; set; }
    }
}
