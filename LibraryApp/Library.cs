using Microsoft.VisualBasic;

namespace LibraryApp;

public class Library
{
    private List<Book> bookslist;
    private IFileManager fileManager;


    public Library()
    {
        fileManager = new JsonFileManager();
        bookslist = fileManager.Load<Book>();
    }

    private void SaveChanges()
    {
        fileManager.Save(bookslist);
    }

    
    public void AddBook(string title, string author, int year, int id)
    {
        bookslist.Add(new Book(title, author, year, id, BookStatus.Free));
        SaveChanges();
    }
    
    public void DeleteBook(int id)
    {
        var book = bookslist.FirstOrDefault(book => book.Id == id);
        if (book == null)
            throw new Exception("Book not found");
        
        bookslist.Remove(book);
        SaveChanges();
    }

    public Book GetBookById(int id)
    {
        var book = bookslist.FirstOrDefault(book => book.Id == id);
        if (book == null)
            throw new Exception("Book not found");
        
        return book;
    }

    public Book GetBookByAuthor(string author)
    {
        var book = bookslist.FirstOrDefault(book => book.Author == author);
        if (book == null)
            throw new Exception("Book not found");
        
        return book;
    }

    public Book GetBookByTitle(string title)
    {
        var book = bookslist.FirstOrDefault(book => book.Title == title);
        if (book == null)
            throw new Exception("Book not found");
        
        return book;
    }

    public List<Book> GetAllBooks()
    {
        return bookslist;
    }

    public void BorrowBook(int id)
    {
        var book = bookslist.FirstOrDefault(book => book.Id == id);
        if (book == null)
            throw new Exception("Book not found");
        book.Status = BookStatus.Busy;
        SaveChanges();
    }

    public void ReturnBook(int id)
    {
        var book = bookslist.FirstOrDefault(book => book.Id == id);
        if (book == null)
            throw new Exception("Book not found");
        book.Status = BookStatus.Free;
        SaveChanges();
    }


}