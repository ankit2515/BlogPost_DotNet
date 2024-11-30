using BlogPost.web.Models;
using X.PagedList;

namespace BlogPost.web.ViewModel
{
    public class HomeVM
    {
        public string? Title { get; set; }
        public string? shortDescription { get; set; }

        public string? ThumbnailUrl { get; set; }
        public IPagedList<Post>? Posts { get; set; }
    }
}
