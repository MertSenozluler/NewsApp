using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsApp.Models.Domain;
using NewsApp.Models.DTO;
using NewsApp.Repositories.Abstract;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace NewsApp.Repositories.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly DatabaseContext ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(DatabaseContext ctx, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            _userManager = userManager;
            // this is for getting login userid
            _httpContextAccessor = httpContextAccessor;
        }
        public bool Add(Comments model, string userId, string NewsId)
        {
            try
            {
                var user = ctx.Users.Find(userId);
                model.User = user;
                model.UserId = userId;
                model.CommentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                ctx.Comments.Add(model);
                    ctx.SaveChanges();
                    return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete (int CommentId, string CommentUserId)
        {

            try
            {
                var comment = ctx.Comments.FirstOrDefault(x => x.Id == CommentId);
                if (comment == null || comment.UserId == null) return false;



                var CommentUser = ctx.Users.Find(CommentUserId);
                if (CommentUser == null) return false;


                // login user id received
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                // also admin can delete every comment
                if (CommentUserId == userId || _httpContextAccessor.HttpContext.User.IsInRole("admin"))
                {
                    ctx.Comments.Remove(comment);
                    ctx.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<NewsCommentViewModel> List(int id)
        {
            var news = ctx.News.FirstOrDefault(x=>x.Id==id);
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comments = ctx.Comments.Where(x => x.NewsId == news.Id).Select(x => new NewsCommentViewModel
            {
                NewsId = news.Id,
                NewsTitle = news.Title,
                NewsBody = news.Body,
                NewsDate = news.Date,
                NewsImage = news.NewsImage,
                Comment = x.Comment,
                CommentDate = x.CommentDate,
                CommentUser = x.User != null ? x.User : new ApplicationUser(),
                CommentId = x.Id,
                UserId = userId



            }).ToList();
            if (comments.Count() == 0)
            {
                List<NewsCommentViewModel> result = new List<NewsCommentViewModel>
                {
                    new NewsCommentViewModel
                {
                    NewsId = news.Id,
                    NewsTitle = news.Title,
                    NewsBody = news.Body,
                    NewsDate = news.Date,
                    NewsImage = news.NewsImage,
                }
            };
                
                return result;
            }
            else
            {
                return comments;
            }
        }
    }
}
