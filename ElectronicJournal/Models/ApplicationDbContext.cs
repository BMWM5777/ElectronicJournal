using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ElectronicJournal.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GradesDate> GradesDates { get; set; }
        public DbSet<SubjectGroup> subjectGroups { get; set; }
    }
}
