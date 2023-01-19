using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NewsApp.Models.Domain
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Body { get; set; }
        public string? Date { get; set; }
 
        public string? NewsImage { get; set; }  // stores movie image name with extension (eg, image0001.jpg)

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        [Required]
        public List<int>? Categories { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }

        [NotMapped]
        public string? CategoryNames { get; set; }

        [NotMapped]
        public MultiSelectList ? MultiCategoryList { get; set; }




    }
}
