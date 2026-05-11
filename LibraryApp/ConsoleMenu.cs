using System.Collections.Concurrent;

namespace LibraryApp;

public class ConsoleMenu
{
    string log = "";

    public void Run()
    {
        var library = new Library();

        while (true)
        {
            try
            {
                ShowMenu();
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ShowBooks(library.GetAllBooks());
                        log = "";
                        break;
                    case 2:
                    {
                        Console.Write("Название: ");
                        string title = Console.ReadLine();
                        Console.Write("Автор: ");
                        string author = Console.ReadLine();
                        Console.Write("Год: ");
                        int year = int.Parse(Console.ReadLine());
                        Console.Write("Id: ");
                        int id = int.Parse(Console.ReadLine());

                        library.AddBook(title, author, year, id);
                        log = "Book added successfully";
                        break;
                    }
                    case 3:
                    {
                        Console.Write("Id: ");
                        int id = int.Parse(Console.ReadLine());
                        library.DeleteBook(id);
                        log = "Book deleted successfully";
                        break;
                    }
                    case 4:
                    {
                        Console.Write("Название: ");
                        string title = Console.ReadLine();
                        ShowBooks(library.GetBookByTitle(title));
                        log = "";
                        break;
                    }
                    case 5:
                    {
                        Console.Write("Автор: ");
                        string author = Console.ReadLine();
                        ShowBooks(library.GetBookByAuthor(author));
                        log = "";
                        break;
                    }
                    case 6:
                    {
                        Console.Write("Id: ");
                        int id = int.Parse(Console.ReadLine());
                        library.BorrowBook(id);
                        log = "Book borrowed successfully";
                        break;
                    }
                    case 7:
                    {
                        Console.Write("Id: ");
                        int id = int.Parse(Console.ReadLine());
                        library.ReturnBook(id);
                        log = "Book returned successfully";
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
                log = e.Message;
            }
        }

    }


    private void ShowBooks(Book book)
    {
        Console.WriteLine("-----------------------");
        Console.WriteLine($"{book.Title} - {book.Author} {book.Year}, id:{book.Id} {book.Status}");
        Console.WriteLine("-----------------------");
        Console.ReadLine();
    }

    private void ShowBooks(List<Book> books)
    {
        Console.WriteLine("-----------------------");
        foreach (var book in books)
            Console.WriteLine($"{book.Title} - {book.Author} {book.Year}, id:{book.Id} {book.Status}");
        Console.WriteLine("-----------------------");
        Console.ReadLine();
    }

    private void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("-----------------------");
        Console.WriteLine("Библиотека");
        Console.WriteLine("1. Показать все книги");
        Console.WriteLine("2. Добавить книгу");
        Console.WriteLine("3. Удалить книгу");
        Console.WriteLine("4. Поиск по названию");
        Console.WriteLine("5. Поиск по автору");
        Console.WriteLine("6. Взять книгу");
        Console.WriteLine("7. Вернуть книгу");
        Console.WriteLine("0. Выход");
        Console.WriteLine("-----------------------");
        Console.WriteLine(log);
        Console.Write("Выбор: ");
    }

}