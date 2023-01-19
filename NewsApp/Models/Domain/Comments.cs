using System.ComponentModel.DataAnnotations;

namespace NewsApp.Models.Domain
{
    public class Comments
    {
        public int Id { get; set; }

        public string? Date { get; set; }
        public int? NewsId { get; set; }
        
        public string? CommentDate { get; set; }
        public string Comment { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
