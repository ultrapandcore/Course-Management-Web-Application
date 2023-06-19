namespace University
{
    public class DailySchedule
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Lecture> Lectures { get; set; } = new();

        public int PersonId { get; set; }
        public Person? Person { get; set; }
    }

}