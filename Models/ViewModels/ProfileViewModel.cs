namespace DogGo.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Owner Owner { get; set; }

        public Walker Walker { get; set; }

        public List<Walk> Walk { get; set; }

        public List<Walker> Walkers { get; set; }

        public List<Doggo> Doggos { get; set; }

    }
}
