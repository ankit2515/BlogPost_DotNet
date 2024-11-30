namespace BlogPost.web.Models.Domain
{
    public class Tags
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? DisplayName { get; set; }

        public ICollection<Blogs> Blogs { get; set; }
    }
}
