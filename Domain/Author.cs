using System.Text.Json.Serialization;

namespace Domain
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Book> Books { get; set; } = new();
        public Author() { }
        public Author(int id, string name)
        {
            Id = id;
            Name = name;
            Books = new List<Book>();
        }
    }
}
