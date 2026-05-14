using System.Collections.Concurrent;

namespace LibraryApp;

public class ConsoleMenu
{
    public void Run()
    {
        var library = new Library();

        while (true)
        {
            try
            {
                ShowMenu();
                if(!int.TryParse(Console.ReadLine(), out int choice))
                    throw new Exception("Wrong choice!");
                switch (choice)
                {
                    case 1:
                        ShowBooks(library.GetAllBooks());
                        break;
                    case 2:
                    {
                        Console.Write("Book name: ");
                        string title = Console.ReadLine();
                        if(string.IsNullOrEmpty(title))
                            throw  new Exception("Book name cannot be empty!");
                        Console.Write("Author: ");
                        string author = Console.ReadLine();
                        if(string.IsNullOrEmpty(author))
                            throw  new Exception("Author cannot be empty!");
                        
                        Console.Write("Year: ");
                        if(!int.TryParse(Console.ReadLine(), out int year))
                            throw new Exception("Wrong year value!");
                        
                        Console.Write("Id: ");
                        if(!int.TryParse(Console.ReadLine(), out int id))
                            throw new Exception("Wrong id value!");

                        library.AddBook(title, author, year, id);
                        Console.WriteLine("Book added successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 3:
                    {
                        Console.Write("Id: ");
                        if(!int.TryParse(Console.ReadLine(), out int id))
                            throw new Exception("Wrong id value!");
                        
                        library.DeleteBook(id);
                        Console.WriteLine("Book deleted successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 4:
                    {
                        Console.Write("Title: ");
                        string title = Console.ReadLine();
                        if(string.IsNullOrEmpty(title))
                            throw  new Exception("Title cannot be empty!");
                        
                        ShowBooks(library.GetBookByTitle(title));
                        break;
                    }
                    case 5:
                    {
                        Console.Write("Author: ");
                        string author = Console.ReadLine();
                        if(string.IsNullOrEmpty(author))
                            throw  new Exception("Author cannot be empty!");
                        
                        ShowBooks(library.GetBookByAuthor(author));
                        break;
                    }
                    case 6:
                    {
                        Console.Write("Id: ");
                        if(!int.TryParse(Console.ReadLine(), out int id))
                            throw new Exception("Wrong id value!");
                        
                        library.BorrowBook(id);
                        Console.WriteLine("Book borrowed successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 7:
                    {
                        Console.Write("Id: ");
                        if(!int.TryParse(Console.ReadLine(), out int id))
                            throw new Exception("Wrong id value!");
                        
                        library.ReturnBook(id);
                        Console.WriteLine("Book returned successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 0:
                        return;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

    }


    private void ShowBooks(Book book)
    {
        Console.WriteLine("-----------------------");
        Console.WriteLine(book);
        Console.WriteLine("-----------------------");
        Console.ReadLine();
    }

    private void ShowBooks(List<Book> books)
    {
        Console.WriteLine("-----------------------");
        foreach (var book in books)
            Console.WriteLine(book);
        Console.WriteLine("-----------------------");
        Console.ReadLine();
    }

    private void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("-----------------------");
        Console.WriteLine("Library Menu");
        Console.WriteLine("1. Show Books");
        Console.WriteLine("2. Add Book");
        Console.WriteLine("3. Delete Book");
        Console.WriteLine("4. Search Books by Title");
        Console.WriteLine("5. Search Books by Author");
        Console.WriteLine("6. Take book");
        Console.WriteLine("7. Return Book");
        Console.WriteLine("0. Exit");
        Console.WriteLine("-----------------------");
        Console.Write("Make choice: ");
    }

}