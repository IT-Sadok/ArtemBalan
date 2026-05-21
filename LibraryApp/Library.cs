using System.Collections.Concurrent;
using Microsoft.VisualBasic;

namespace LibraryApp;

public class Library
{
    private Random rnd = new Random();
    private int logMaxLen = 10;
    private int taskAliveCounter = 0;
    private Lock bookLocker = new Lock();
    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
    private List<Book> bookslist;
    private IFileManager fileManager;
    private BookGenerator bookGenerator;
    private CancellationTokenSource cts = new();
    
    public int TaskAliveCounter => taskAliveCounter;
    public int BooksCount => bookslist.Count;
    
    public void StopTasks()
    {
        cts.Cancel();
    }

    public void DoRandom()
    {
        string msg = "";
        int rndSelector = rnd.Next(0, 2);
        Book book;
        switch (rndSelector)
        {
            case 0: // add book
                book = bookGenerator.GenerateBook();
                lock (bookLocker)
                    AddBook(book);
                msg = $"Thread: {Environment.CurrentManagedThreadId.ToString()} added new book {book.Id}";
                LogAdd(msg);
                break;
            case 1: // delete random
                lock (bookLocker)
                {
                    if (bookslist.Count() <= 1)
                        break;
                    book = bookslist[new Random().Next(0, bookslist.Count)];
                    bookslist.Remove(book);
                }
                msg = $"Thread: {Environment.CurrentManagedThreadId.ToString()} deleted book {book.Id}";
                LogAdd(msg);
                break;
        }
    }

    public void TaskStarter(int taskCount)
    {
        cts=new CancellationTokenSource(); 
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < taskCount; i++)
        {
            Task task = Task.Run(() =>
            {
                Interlocked.Increment(ref taskAliveCounter);
                while (!cts.Token.IsCancellationRequested)
                {
                    DoRandom();
                    Task.Delay(1000).Wait();
                }
                Interlocked.Decrement(ref taskAliveCounter);
            }, cts.Token);
            tasks.Add(task);
        }
        Task.WhenAll(tasks);
    }

    public void LogAdd(string logMsg)
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        logQueue.Enqueue($"[{time}] {logMsg}");
    }

    public List<string> GetLogs()
    {
        List<string> _logHistory = new();
        while (logQueue.TryDequeue(out string log))
        {
            _logHistory.Add(log);
            //if(_logHistory.Count > 100)
                //break;
        }
        return _logHistory;
    }

    private Library(JsonFileManager  fileManager)
    {
        this.fileManager = fileManager;
    }

    public Library(): this(new JsonFileManager())
    {
        bookslist = fileManager.Load<Book>();
    }

    public Library(int generateRandomBooksCount): this(new JsonFileManager())
    {
        bookGenerator = new BookGenerator();
        bookslist = bookGenerator.GenerateBooks(generateRandomBooksCount);
        SaveChanges();
    }

    private void SaveChanges()
    {
        fileManager.Save(bookslist);
    }

    public void AddBook(Book book)
    {
        bookslist.Add(book);
        SaveChanges();
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