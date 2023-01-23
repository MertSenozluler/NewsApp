using NewsApp.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NewsApp.Models.DTO
{
    public class NewsCommentViewModel
    {
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsBody { get; set; }
        public string NewsDate { get; set; }
        public string NewsImage { get; set; }
        public int CommentId { get; set; }
        public string Comment { get; set; }
        public string CommentDate { get; set; }
        public ApplicationUser CommentUser { get; set; }
        public string UserId { get; set; }
    }
}
