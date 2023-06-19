using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace University.Data.EF
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniversityContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UniversityContext>>()))
            {
                // Look for any Groups.
                if (!context.Groups.Any())
                {
                    var sr01 = new Group
                    {
                        Name = "SR-01"
                    };

                    var sr02 = new Group
                    {
                        Name = "SR-02"
                    };

                    var sr03 = new Group
                    {
                        Name = "SR-03"
                    };

                    var sr04 = new Group
                    {
                        Name = "SR-04"
                    };

                    context.Groups.AddRange(sr01, sr02, sr03, sr04);
                    context.SaveChanges();

                    // Look for any Courses.
                    if (!context.Courses.Any())
                    {
                        context.Courses.AddRange(
                        new Course
                        {
                            Name = "Arts",
                            Description = "become an artist",
                            Groups = new List<Group> { sr01, sr02 }
                        },

                        new Course
                        {
                            Name = "Dancing",
                            Description = "cool moves",
                            Groups = new List<Group> { sr02, sr03 }
                        },

                        new Course
                        {
                            Name = "Archeology",
                            Description = "ancient adventures",
                            Groups = new List<Group> { sr03, sr04 }
                        },

                        new Course
                        {
                            Name = "Cosmology",
                            Description = "lets find Reapers",
                            Groups = new List<Group> { sr04, sr01 }
                        });
                        context.SaveChanges();
                    }
                }

                // Look for any Students.
                if (!context.Students.Any())
                {
                    context.Students.AddRange(
                        new Student
                        {
                            FirstName = "Steve",
                            LastName = "Rogers",
                            PhoneNumber = "(555) 555-1234",
                            Email = "steve.rogers@unimail.com",
                            GroupId = 1,
                            AverageMark = 5m
                        },
                        new Student
                        {
                            FirstName = "Tony",
                            LastName = "Stark",
                            PhoneNumber = "(555) 555-1235",
                            Email = "tony.stark@unimail.com",
                            GroupId = 2,
                            AverageMark = 4m
                        },
                        new Student
                        {
                            FirstName = "Thor",
                            LastName = "Odinson",
                            PhoneNumber = "(555) 555-1294",
                            Email = "thor.odinson@unimail.com",
                            GroupId = 3,
                            AverageMark = 3.5m
                        },
                        new Student
                        {
                            FirstName = "Natasha",
                            LastName = "Romanoff",
                            PhoneNumber = "(555) 555-1694",
                            Email = "nata.romanoff@unimail.com",
                            GroupId = 4,
                            AverageMark = 5m
                        },
                        new Student
                        {
                            FirstName = "Clint",
                            LastName = "Barton",
                            PhoneNumber = "(555) 555-8994",
                            Email = "clint.barton@unimail.com",
                            GroupId = 4,
                            AverageMark = 3m
                        });
                    context.SaveChanges();
                }
            }
        }
    }
}