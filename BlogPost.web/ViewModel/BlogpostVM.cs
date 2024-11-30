namespace BlogPost.web.ViewModel
{
    public class BlogpostVM
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ThumbnailUrl { get; set; }

        public string? ShortDesscription {  get; set; } 
        public string? Description { get; set; } 
    }
}
