using NewsApp.Models.Domain;

namespace NewsApp.Models.DTO
{
    public class NewsListViewModel
    {
        public IQueryable<News> NewsList { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Term { get; set; }
    }
}
