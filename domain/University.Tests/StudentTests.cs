using Xunit;

namespace University.Tests
{
    public class StudentTests
    {
        [Fact]
        public void AverageMarkShouldReturnCorrectValue()
        {
            var marks = new List<Mark>();
            marks.Add(new Mark { StudentId = 1, LectureId = 1, Grade = 5 });
            marks.Add(new Mark { StudentId = 1, LectureId = 2, Grade = 3 });
            marks.Add(new Mark { StudentId = 1, LectureId = 3, Grade = 3 });

            var student = new Student { FirstName = "", LastName = "", PhoneNumber = "", Marks = marks };

            var expected = ((decimal)(marks[0].Grade + marks[1].Grade + marks[2].Grade) / marks.Count);

            expected = Decimal.Round(expected, 2);
            var actual = student.AverageMark;
            Assert.Equal(expected, actual);
        }
    }
}
