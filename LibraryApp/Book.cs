namespace LibraryApp;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public int Id { get; set; }
    public BookStatus Status { get; set; }

    public Book(string title = "Default", string author = "None", int year = 0, int id = -1,
        BookStatus status = BookStatus.Free)
    {
        Title = title;
        Author = author;
        Year = year;
        Id = id;

        Status = status;
    }
}