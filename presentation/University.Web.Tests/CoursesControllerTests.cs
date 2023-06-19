using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using University.Data.EF;
using University.Web.Controllers;
using University.Web.MappingConfigurations;
using University.Web.Models;
using Xunit;

namespace University.Web.Tests
{
    public class CoursesControllerTests
    {
        [Fact]
        public void DeleteConfirmed_ShouldThrowInvalidOperationException_WhenThereIsGroupInCourse()
        {
            var groups = new List<Group>();
            groups.Add(new Group { Id = 1, Name = "TestGroup" });

            var courseId = 1;
            var course = new Course { Id = courseId, Name = "TestCourse", Description = "test", Groups = groups };

            var mockSet = new Mock<DbSet<Course>>();
            var mockContext = new Mock<UniversityContext>();
            mockContext.Setup(m => m.Courses).Returns(mockSet.Object);

            var myProfile = new MapProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var courseModel = mapper.Map<CourseModel>(course);

            var controller = new CoursesController(mockContext.Object, mapper);
            controller.Create(courseModel);

            var result = controller.DeleteConfirmed(courseId);

            Assert.ThrowsAsync<InvalidOperationException>(() => result);
        }
    }
}