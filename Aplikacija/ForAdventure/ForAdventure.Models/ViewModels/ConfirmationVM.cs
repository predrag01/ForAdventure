namespace ForAdventure.Models.ViewModels
{
    public class ConfirmationVM
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public double TotalPrice { get; set; }
    }
}
