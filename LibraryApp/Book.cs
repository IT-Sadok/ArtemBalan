namespace LibraryApp;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public int Id { get; set; }
    public BookStatus Status { get; set; }

    public override string ToString()
    {
        return $"{Title} - {Author} {Year}, id:{Id} {Status}";
    }

    public Book(string title , string author , int year , int id ,
        BookStatus status )
    {
        Title = title;
        Author = author;
        Year = year;
        Id = id;
        Status = status;
    }
}