namespace LibraryApp;

public class BookGenerator
{
    private Random random = new Random();
    public List<Book> GenerateBooks(int count)
    {
        var books = new List<Book>();
        for (int i = 0; i < count; i++)
        {
            string title = GetRandomTitle();
            string author = GetRandomAuthor();
            int year = random.Next(1900, 2026);
            books.Add(new Book(title, author, year, i, BookStatus.Free));
        }
        return books;
    }

    private string GetRandomAuthor()
    {
        return LibraryData.Names[random.Next(LibraryData.Names.Length)] +
               " " +
               LibraryData.SecondNames[random.Next(LibraryData.SecondNames.Length)];
    }

    private string GetRandomTitle()
    {
        return LibraryData.BookTitleFirstPart[random.Next(LibraryData.BookTitleFirstPart.Length)]+
               " "+
               LibraryData.BookTitleSecondPart[random.Next(LibraryData.BookTitleSecondPart.Length)];
    }
}