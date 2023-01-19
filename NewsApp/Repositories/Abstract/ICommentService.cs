using NewsApp.Models.Domain;
using NewsApp.Models.DTO;

namespace NewsApp.Repositories.Abstract
{
    public interface ICommentService
    {
        bool Add(Comments model,string userId, string NewsId);
        Task<bool> Delete(int CommentId, string CommentUserId);
        List<NewsCommentViewModel> List(int id);
    }
}
