using Microsoft.AspNetCore.Mvc;
using NewsApp.Models;
using NewsApp.Models.Domain;
using NewsApp.Repositories.Abstract;
using System.Collections.Generic;
using System.Diagnostics;

namespace NewsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;


        public HomeController(INewsService newsService, ICategoryService categoryService, ICommentService commentService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
            _commentService = commentService;
        }


        public IActionResult Index(string term = "")
        {
            var news = _newsService.List(term);
            if (string.IsNullOrEmpty(term))
            {
                return View(news);
            }
            else
            {
                return View("SearchResult", news);
            }
        }


        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult NewsDetail(int NewsId)
        {
            var newsAndComments=_commentService.List(NewsId);
            return View(newsAndComments);
        }



        public IActionResult GetNewsByCategoryId(int categoryId)
        {
            var result = _newsService.GetNewsByCategoryId(categoryId);

            return View(result);
        }



    }
}
