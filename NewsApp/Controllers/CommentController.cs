using Microsoft.AspNetCore.Mvc;
using NewsApp.Models.Domain;
using NewsApp.Models.DTO;
using NewsApp.Repositories.Abstract;
using NewsApp.Repositories.Implementation;

namespace NewsApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ApplicationUser applicationUser;
        private readonly NewsCommentViewModel newsCommentViewModel;
        private readonly ICommentService _commentService;


        public CommentController(DatabaseContext context, ApplicationUser applicationUser, NewsCommentViewModel newsCommentViewModel, ICommentService commentService)
        {
            _context = context;
            this.applicationUser = applicationUser;
            this.newsCommentViewModel = newsCommentViewModel;
            _commentService = commentService;
        }


        [HttpPost]
        public IActionResult Add(Comments model, string userId, string NewsId)
        {

            var result = _commentService.Add(model, userId, NewsId);
            if (result)
            {
                TempData["msg"] = "Added successfully";
                return RedirectToAction("NewsDetail", "Home", model);
            }
            else
            {
                TempData["msg"] = "Error on the server side...";
                return RedirectToAction("NewsDetail", "Home", model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int CommentId, string CommentUserId, Comments model)
        {
            var result = _commentService.Delete(CommentId, CommentUserId);
            return RedirectToAction("NewsDetail", "Home", model);
            //return RedirectToAction("Index", "Home");
        }


    }
}
