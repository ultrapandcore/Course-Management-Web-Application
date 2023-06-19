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
    public class GroupsControllerTests
    {

        [Fact]
        public void DeleteConfirmed_ShouldThrowInvalidOperationException_WhenThereIsStudentInGroup()
        {
            var students = new List<Student>();
            students.Add(new Student { Id = 1, FirstName = "", LastName = "", PhoneNumber = "" });

            var groupId = 1;
            var group = new Group { Id = groupId, Name = "", Students = students };

            var mockSet = new Mock<DbSet<Group>>();
            var mockContext = new Mock<UniversityContext>();
            mockContext.Setup(m => m.Groups).Returns(mockSet.Object);

            var myProfile = new MapProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var groupModel = mapper.Map<GroupModel>(group);

            var controller = new GroupsController(mockContext.Object, mapper);
            controller.Create(groupModel, null);

            var result = controller.DeleteConfirmed(groupId);

            Assert.ThrowsAsync<InvalidOperationException>(() => result);
        }
    }
}