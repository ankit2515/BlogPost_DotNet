using BlogPost.web.Data;
using BlogPost.web.Models;
using BlogPost.web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList.Extensions;

namespace BlogPost.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _blogDbContext;

        public HomeController(ILogger<HomeController> logger, BlogDbContext _blogDbContext)
        {
            _logger = logger;
            this._blogDbContext = _blogDbContext;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var vm = new HomeVM();
            var setting = _blogDbContext.settings!.ToList();
            vm.Title = setting[0].Title;
            vm.shortDescription = setting[0].ShortDescription;
            vm.ThumbnailUrl = setting[0].ThumbnailUrl;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            vm.Posts =  _blogDbContext.posts!.Include(x=>x.ApplicationUser).OrderByDescending(x=>x.CreatedDate).ToPagedList(pageNumber,pageSize); //for including application user details at home page
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
