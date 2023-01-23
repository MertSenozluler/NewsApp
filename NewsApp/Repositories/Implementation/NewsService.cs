using Microsoft.EntityFrameworkCore;
using NewsApp.Models.Domain;
using NewsApp.Models.DTO;
using NewsApp.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace NewsApp.Repositories.Implementation
{
    public class NewsService : INewsService
    {
        private readonly DatabaseContext ctx;
        public NewsService(DatabaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(News model)
        {
            try
            {
                if (model.Title.Length < 5 || model.Title.Length > 20)
                {
                    return false;
                }
                ctx.News.Add(model);
                ctx.SaveChanges();
                foreach (var categoryId in model.Categories)
                {
                    var newsCategory = new NewsCategory
                    {
                        NewsId = model.Id,
                        CategoryId = categoryId
                    };
                    ctx.NewsCategory.Add(newsCategory);

                }
                ctx.SaveChanges() ;
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null) return false;
                var newsCategories = ctx.NewsCategory.Where(a => a.NewsId == data.Id);
                foreach (var newsCategory in newsCategories)
                {
                    ctx.NewsCategory.Remove(newsCategory);
                }
                ctx.News.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {

               return false;
            }
        }

        public News GetById(int id)
        {
            return ctx.News.Find(id);
        }

       

        public NewsListViewModel List(string term = "", bool paging = false, int currentPage = 0)
        {
            var data=new NewsListViewModel();
            var list = ctx.News.ToList();

            //search button
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().Contains(term) || a.Body.ToLower().Contains(term)).ToList();
            }

            // pagination
            if (paging)
            {
                int pageSize = 5;
                int count = list.Count;
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;

            }

            foreach (var news in list)
            {
                var categories=(from category in ctx.Category
                                join nc in ctx.NewsCategory
                                on category.Id equals nc.CategoryId
                                where nc.NewsId == news.Id
                                select category.CategoryName).ToList();
                var categoryNames = string.Join(',', categories);
                news.CategoryNames = categoryNames;
            }
            data.NewsList = list.AsQueryable();
            return data;
        }

        public bool Update(News model)
        {
            try
            {
                // these categoryIds are not selected by users and still present is newsCategory table corresponding to
                // this newsId. So these ids should be removed.
                var categoryToDelete = ctx.NewsCategory.Where(a => a.NewsId == model.Id && !model.Categories.Contains(a.CategoryId)).ToList();

                foreach(var nCategory in categoryToDelete)
                {
                    ctx.NewsCategory.Remove(nCategory);
                }

                foreach(int catId in model.Categories)
                {
                    var newsCategory = ctx.NewsCategory.FirstOrDefault(a => a.NewsId == model.Id && a.CategoryId == catId);
                    if (newsCategory == null)
                    {
                        newsCategory = new NewsCategory{CategoryId = catId, NewsId = model.Id};
                        ctx.NewsCategory.Add(newsCategory);
                    }
                }
                
                ctx.News.Update(model);
                // we have to add these categoy ids in newsCategory table
                model.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public List<int> GetCategoryByNewsId(int newsId)
        {

            var categoryIds = ctx.NewsCategory.Where(a => a.NewsId == newsId).Select(a => a.CategoryId).ToList();
            return categoryIds;
        }

        public List<News> GetNewsByCategoryId(int categoryId)
        {

            // we have newsIds according to categoryIds
            var newsIds = ctx.NewsCategory.Where(a => a.CategoryId == categoryId).Select(a => a.NewsId).ToList();

            var newsList = ctx.News.Where(n => newsIds.Contains(n.Id)).ToList();

            return newsList;

            
        }
    }
}
