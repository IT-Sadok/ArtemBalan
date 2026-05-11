using Microsoft.VisualBasic;

namespace LibraryApp;

public class Library
{
    private Dictionary<int, Book> booksDictionary;


    public Library()
    {
        var books = JsonFileManager.Load<Book>();
        booksDictionary = books.ToDictionary(book => book.Id, book => book);
    }

    private void SaveChanges()
    {
        JsonFileManager.Save(booksDictionary.Values.ToList());
    }

    
    public void AddBook(string title, string author, int year, int id)
    {
        booksDictionary.Add(id, new Book(title, author, year, id, BookStatus.Free));
        SaveChanges();
    }
    
    public void DeleteBook(int id)
    {
        booksDictionary.Remove(id);
        SaveChanges();
    }

    public Book GetBookById(int id)
    {
        return booksDictionary[id];
    }

    public Book GetBookByAuthor(string author)
    {
        return booksDictionary.Values.FirstOrDefault(book => book.Author == author) ?? new Book();
    }

    public Book GetBookByTitle(string title)
    {
        return booksDictionary.Values.FirstOrDefault(book => book.Title == title) ?? new Book();
    }

    public List<Book> GetAllBooks()
    {
        return booksDictionary.Values.ToList();
    }

    public void BorrowBook(int id)
    {
        booksDictionary[id].Status = BookStatus.Busy;
        SaveChanges();
    }

    public void ReturnBook(int id)
    {
        booksDictionary[id].Status = BookStatus.Free;
        SaveChanges();
    }


}