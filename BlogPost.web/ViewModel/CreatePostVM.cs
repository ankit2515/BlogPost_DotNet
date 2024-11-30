using BlogPost.web.Models;

namespace BlogPost.web.ViewModel
{
    public class CreatePostVM
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        //relation  (one post can only one application user)
        public string? ApplicationUserId { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail {  get; set; }
       
    }
}
