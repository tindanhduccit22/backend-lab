namespace lab4.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BookCode { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishedYear { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public IFormFile AvatarFile { get; set; }
        public IFormFile PdfFile { get; set; }
    }
}