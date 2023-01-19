using NewsApp.Models.Domain;
using NewsApp.Models.DTO;

namespace NewsApp.Repositories.Abstract
{
    public interface INewsService
    {
        bool Add(News model);
        bool Update(News model);
        News GetById(int id);
        bool Delete(int id);
        NewsListViewModel List(string term = "", bool paging = false, int currentPage = 0);
        List<int>GetCategoryByNewsId(int newsId);
        List<News> GetNewsByCategoryId(int categoryId);
    }
}
