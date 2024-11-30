namespace BlogPost.web.ViewModel
{
    public class PostVM
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName{ get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ThumbnailUrl { get; set; }

    }
}
