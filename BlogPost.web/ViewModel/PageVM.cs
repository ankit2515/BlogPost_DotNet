using System.ComponentModel.DataAnnotations;

namespace BlogPost.web.ViewModel
{
    public class PageVM
    {
        public Guid Id { get; set; }

        [Required]
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }

        public string? ThumbnailUrl {  get; set; }

        public IFormFile? Thumbnail { get; set; }
    }
}
