using Microsoft.AspNetCore.Mvc;
using NewsApp.Models.Domain;

namespace NewsApp.Views.Shared.ViewComponents
{
    public class CategoryListViewComponent:ViewComponent
    {
        private readonly DatabaseContext _context;

        public CategoryListViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult>InvokeAsync()
        {
            var categories = _context.Category.Select(x=>
                new Category()
                {   Id=x.Id,
                    CategoryName=x.CategoryName }).ToList();

            return View(categories);
        }
    }
}
