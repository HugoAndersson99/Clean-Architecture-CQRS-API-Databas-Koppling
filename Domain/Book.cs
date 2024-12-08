using System.Text.Json.Serialization;

namespace Domain
{
    public class Book
    {
        public Book(int id, string title, string description, Author author = null)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
            AuthorId = author?.Id ?? 0;
        }

        public Book() { }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        
        public Author Author { get; set; }

        
        [JsonIgnore]
        public int AuthorId { get; set; }
    }
}
