using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsApp.Repositories.Abstract;
using NewsApp.Repositories.Implementation;
using NewsApp.Models.Domain;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace NewsApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewsController : Controller
    {

        private readonly INewsService _newsService;
        private readonly IFileService _fileService;
        private readonly ICategoryService _catService;

        public NewsController(INewsService newsService, IFileService fileService, ICategoryService catService)
        {
            this._newsService = newsService;
            _fileService = fileService;
            _catService = catService;
        }

      
        public IActionResult Add()
        {
            var model = new News();
            model.CategoryList = _catService.List().
                Select(a => new SelectListItem
                {
                    Text = a.CategoryName,
                    Value = a.Id.ToString()
                });
            return View(model);
            
        }
        
        [HttpPost]
        public IActionResult Add(News model)
        {
            model.CategoryList = _catService.List().
                Select(a => new SelectListItem
                {
                    Text = a.CategoryName,
                    Value = a.Id.ToString()
                });
            model.Date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if(model.ImageFile!= null)
            {
                var fileResult = this._fileService.
                    SaveImage(model.ImageFile);
                if(fileResult.Item1==0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.NewsImage = imageName;
            }
            var result = _newsService.Add(model);
            if(result)
            {
                TempData["msg"] = "Added successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on the server side...";
                return View(model);
            }

        }
  
        public IActionResult Edit(int id)
        {
            var model = _newsService.GetById(id);
            var selectedCategories = _newsService.GetCategoryByNewsId(model.Id);
            MultiSelectList multiCategoryList = new MultiSelectList(_catService.List(),"Id", "CategoryName", selectedCategories);
            model.MultiCategoryList= multiCategoryList;
            return View(model);
        }
    
        [HttpPost]
        public IActionResult Edit(News model) 
        {
            var selectedCategories = _newsService.GetCategoryByNewsId(model.Id);
            MultiSelectList multiCategoryList = new MultiSelectList(_catService.List(),
                "Id", "CategoryName", selectedCategories);
            model.MultiCategoryList= multiCategoryList;
            if(!ModelState.IsValid)  return View(model); 
            if(model.ImageFile != null)
            {
                var fileResult = this._fileService.SaveImage(model.ImageFile);
                if(fileResult.Item1==0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileResult.Item2;
                model.NewsImage = imageName;
            }
            var result = _newsService.Update(model);
            if (result)
            {
                TempData["msg"] = "Successfully Updated";
                return RedirectToAction(nameof(NewsList));
            }
            else
            {
                TempData["msg"] = "Error on the server side...";
                return View();
            }

        }
  
        public IActionResult NewsList(string term = "", int currentPage = 1)
        {

            var data=this._newsService.List(term, true, currentPage);
            return View(data);
        }
      
        public IActionResult Delete(int id)
        {
            var result = _newsService.Delete(id);
            TempData["msg"] = "Successfully Deleted";
            return RedirectToAction(nameof(NewsList));
        }



        public IActionResult SearchResult(string term = "")
        {
            if (term != "")
            {
                var result = _newsService.List(term);
                return View(result);
            }
            return View();

        }
    }
}
