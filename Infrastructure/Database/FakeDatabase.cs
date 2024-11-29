using Domain;

namespace Infrastructure.Database
{
    public class FakeDatabase
    {
        public List<Book> Books { get; set; } = new();
        public List<Author> Authors { get; set; } = new();

        public FakeDatabase() {
          Author HugoAuthor = new Author(1, "Hugo");
          Author StefanAuthor = new Author(2, "Stefan");
          Author KalleAuthor = new Author(3, "Kalle");
         
          Authors.Add(HugoAuthor);
          Authors.Add(StefanAuthor);
          Authors.Add(KalleAuthor);
         
          Book hugoBook1 = new Book(1, "FirstBookOfHugo", "Komedi", HugoAuthor);
          Book hugoBook2 = new Book(2, "SecondBookOfHugo", "Skräck", HugoAuthor);
          Book stefanBook = new Book(3, "OnlyBookOfStefan", "Fantasy", StefanAuthor);
          Book noAuthorBook = new Book(4, "BookWithNoAuthor", "Fantasy");
         
          HugoAuthor.Books.Add(hugoBook1);
          HugoAuthor.Books.Add(hugoBook2);
          StefanAuthor.Books.Add(stefanBook);
         
          Books.Add(hugoBook1);
          Books.Add(hugoBook2);
          Books.Add(stefanBook);
          Books.Add(noAuthorBook);
        }

        public List<User> Users
        {
            get { return allUsers; }
            set { allUsers = value; }
        }

        private static List<User> allUsers = new List<User>()
        {
            new User { Id = Guid.NewGuid(), UserName = "Hugo"},
            new User { Id = Guid.NewGuid(), UserName = "Tori"},
            new User { Id = Guid.NewGuid(), UserName = "Bali"}
        };
    }
}
