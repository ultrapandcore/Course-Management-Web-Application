using Microsoft.EntityFrameworkCore;

namespace University.Data.EF
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        public UniversityContext() { }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public DbSet<LectureRoom> LectureRooms { get; set; } = null!;
        public DbSet<Lecture> Lectures { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public DbSet<GroupCourse> GroupCourses { get; set; } = null!;
        public DbSet<Mark> Marks { get; set; } = null!;
        public DbSet<DailySchedule> DailySchedules { get; set; } = null!;
        public DbSet<MonthlySchedule> MonthlySchedules { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mark>()
                .HasKey(m => new { m.StudentId, m.LectureId });

            modelBuilder.Entity<Course>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Group>()
                .HasIndex(g => g.Name)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Group>()
                .HasMany<Course>(g => g.Courses)
                .WithMany(c => c.Groups)
                .UsingEntity<GroupCourse>()
                .HasKey(gc => new { gc.GroupId, gc.CourseId });

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Courses)
                .WithMany(c => c.Teachers)
                .UsingEntity(j => j.ToTable("TeacherCourse"));
        }

    }
}
