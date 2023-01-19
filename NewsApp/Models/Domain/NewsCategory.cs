namespace NewsApp.Models.Domain
{
    public class NewsCategory
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public int CategoryId { get; set; }
    }
}
