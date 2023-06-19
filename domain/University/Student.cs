using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace University
{
    [Table("Students")]
    public class Student : Person
    {
        private decimal? _averageMark;

        public int? GroupId { get; set; }
        public Group? Group { get; set; }

        public List<Mark> Marks { get; set; } = new();

        [DisplayName("Average Mark")]
        public decimal? AverageMark
        {
            get
            {
                if (_averageMark == null)
                {
                    return CalculateAverageMark();
                }

                return _averageMark;
            }

            // it's allowed for the sake of cheating :)
            set
            {
                _averageMark = value;
            }
        }

        public MonthlySchedule? Schedule { get; set; }

        private decimal CalculateAverageMark()
        {
            decimal averageMark = 0;

            if (Marks.Count > 0)
            {
                foreach (var Mark in Marks)
                {
                    averageMark += (decimal)Mark.Grade;
                }

                averageMark /= Marks.Count;
            }

            return Decimal.Round(averageMark, 2);
        }
    }

}