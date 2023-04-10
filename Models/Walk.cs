namespace DogGo.Models
{
    public class Walk
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public int DogId { get; set; }

        public Owner Owner { get; set; }

        public TimeSpan DurationTime { get; set; }
    }
}
