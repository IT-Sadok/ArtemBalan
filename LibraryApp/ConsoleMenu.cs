using System.Collections.Concurrent;
using System.Diagnostics;

namespace LibraryApp;

public class ConsoleMenu
{
    Lock consoleLock = new Lock();
    private Library library;
    ConsoleColor foregroundColor = ConsoleColor.White;
    ConsoleColor backgroundColor = ConsoleColor.DarkBlue;
    ConsoleColor backgroundSelectedColor = ConsoleColor.DarkGray;
    ConsoleColor menuHeaderColor = ConsoleColor.Red;
    ConsoleColor menuHeaderBackColor = ConsoleColor.Gray;
    private ConsoleColor logUndertextColor = ConsoleColor.Gray;
    
    List<string> menuElements = new List<string>()
    {
        "Show Books",
        "Add Book",
        "Delete Book",
        "Search Books by Title",
        "Search Books by Author",
        "Take book",
        "Return Book",
        "Start random tasks",
        "Exit"
    };

    public ConsoleMenu()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
    }

    private void LogRender()
    {
        int visibleLines = 13;
        int width = 50;
        int height = 15;
        int left = Console.BufferWidth / 2;
        int top = Console.BufferHeight / 2 - height / 2;
        DrawFrame(left, top, width, height);
        Console.SetCursorPosition(left + 1, top);
        Console.ForegroundColor = menuHeaderColor;
        Console.BackgroundColor = menuHeaderBackColor;
        Console.Write("Log:  'press Enter to stop'");
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        List<string> logs = new List<string>();
        while (true)
        {
            logs.AddRange(library.GetLogs());
            while (logs.Count > visibleLines)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                    return;
                
                Console.SetCursorPosition(left + 1, top + height -1);
                Console.ForegroundColor = logUndertextColor;
                Console.WriteLine($@"Tasks running: {library.TaskAliveCounter} Books count: {library.BooksCount}");
                Console.ForegroundColor = foregroundColor;
                
                Console.SetCursorPosition(left + 1, top + 1);
                for (int i = 0; i < visibleLines; i++)
                {
                    if (i >= logs.Count)
                    {
                        Console.Write(" ".PadRight(width - 1));
                    }
                    else
                    {
                        Console.Write(logs[i]);
                    }
                    Console.SetCursorPosition(left + 1, ++Console.CursorTop);
                }
                logs.RemoveAt(0);
                Task.Delay(30).Wait();
            }
            Task.Delay(100).Wait();
        }
    }

    public async Task Run()
    {
        library = new Library(50);
        while (true)
        {
            try
            {
                int choice = GetMenuSelected("Select action:", menuElements);
                switch (choice)
                {
                    case 0:
                        var books = library.GetAllBooks();
                        RefreshConsoleWindow(false);
                        ShowBooks(books);
                        Console.ReadLine();
                        break;
                    case 1:
                    {
                        string title = InputBox("Book name: ");
                        if (string.IsNullOrEmpty(title))
                            throw new Exception("Book name cannot be empty!");
                        string author = InputBox("Author: ");
                        if (string.IsNullOrEmpty(author))
                            throw new Exception("Author cannot be empty!");
                        if (!int.TryParse(InputBox("Year: "), out int year))
                            throw new Exception("Wrong year value!");
                        if (!int.TryParse(InputBox("Id: "), out int id))
                            throw new Exception("Wrong id value!");
                        library.AddBook(title, author, year, id);
                        MsgBox("Book added successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 2:
                    {
                        if (!int.TryParse(InputBox("Id:"), out int id))
                            throw new Exception("Wrong id value!");
                        library.DeleteBook(id);
                        MsgBox("Book deleted successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 3:
                    {
                        string title = InputBox("Title: ");
                        if (string.IsNullOrEmpty(title))
                            throw new Exception("Title cannot be empty!");
                        ShowBooks(library.GetBookByTitle(title));
                        Console.ReadLine();
                        break;
                    }
                    case 4:
                    {
                        string author = InputBox("Author: ");
                        if (string.IsNullOrEmpty(author))
                            throw new Exception("Author cannot be empty!");
                        ShowBooks(library.GetBookByAuthor(author));
                        Console.ReadLine();
                        break;
                    }
                    case 5:
                    {
                        if (!int.TryParse(InputBox("Id: "), out int id))
                            throw new Exception("Wrong id value!");
                        library.BorrowBook(id);
                        MsgBox("Book borrowed successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 6:
                    {
                        if (!int.TryParse(InputBox("Id: "), out int id))
                            throw new Exception("Wrong id value!");
                        library.ReturnBook(id);
                        MsgBox("Book returned successfully!");
                        Console.ReadLine();
                        break;
                    }
                    case 7:
                    {
                        
                        library.TaskStarter(10);
                        LogRender();
                        library.StopTasks();
                        break;
                    }
                    case 8:
                        return;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                MsgBox(e.Message);
                Console.ReadLine();
            }
        }
    }

    private void ShowBooks(Book book)
    {
        DrawUpperFrame();
        Console.WriteLine("║ " + book.ToString().PadRight(Console.BufferWidth - 1) + "║");
        while (Console.CursorTop <= Console.WindowHeight - 2)
            Console.WriteLine("║ ".PadRight(Console.BufferWidth - 1) + "║");
        DrawBottomFrame();
    }

    private void ShowBooks(List<Book> books)
    {
        DrawUpperFrame();
        foreach (var book in books)
            Console.WriteLine("║ " + book.ToString().PadRight(Console.BufferWidth - 3) + "║");
        while (Console.CursorTop <= Console.WindowHeight - 2)
            Console.WriteLine("║ ".PadRight(Console.BufferWidth - 1) + "║");
        Console.WriteLine("║ ".PadRight(Console.BufferWidth - 1) + "║");
        DrawBottomFrame();
        Console.SetCursorPosition(2, Console.BufferHeight - 1);
        Console.Write("Books total: " + books.Count.ToString());
    }

    private string InputBox(string msg)
    {
        RefreshConsoleWindow();
        int width = Console.WindowWidth / 2;
        int height = 2;
        int left = Console.BufferWidth / 2 - width / 2;
        int top = Console.BufferHeight / 2 - height / 2;

        DrawFrame(left, top, width, height);
        Console.SetCursorPosition(left + 2, top + 1);
        Console.Write(msg);
        string result = Console.ReadLine();
        return result;
    }

    private void MsgBox(string msg)
    {
        RefreshConsoleWindow();
        int width = msg.Length + 3;
        int height = 2;
        int left = Console.BufferWidth / 2 - width / 2;
        int top = Console.BufferHeight / 2 - height / 2;
        DrawFrame(left, top, width, height);
        Console.SetCursorPosition(left + 2, top + 1);
        Console.Write(msg);
    }

    private int GetMenuSelected(string menuHeader, List<string> menus)
    {
        RefreshConsoleWindow();
        int selectedMenuElement = 0;

        int width = menus.Max(item => item.Length) + 2;
        int height = menus.Count() + 1;
        int left = Console.BufferWidth / 3 - width / 2;
        int top = Console.BufferHeight / 2 - height / 2;

        while (true)
        {
            DrawFrame(left, top, width, height);
            
            Console.SetCursorPosition(left + 1, top );
            
            Console.ForegroundColor = menuHeaderColor;
            Console.BackgroundColor = menuHeaderBackColor;
            Console.Write(menuHeader);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor=backgroundColor;
            
            for (int i = 0; i < menus.Count; i++)
            {
                if (i == selectedMenuElement)
                    Console.BackgroundColor = backgroundSelectedColor;
                else
                    Console.BackgroundColor = backgroundColor;

                Console.SetCursorPosition(left + 1, top + 1 + i);
                Console.Write(menus[i]);
            }

            var keyPressed = Console.ReadKey(true);
            if (keyPressed.Key == ConsoleKey.Enter)
                return selectedMenuElement;

            if (keyPressed.Key == ConsoleKey.UpArrow)
            {
                selectedMenuElement--;
                if (selectedMenuElement < 0)
                    selectedMenuElement = 0;
            }

            if (keyPressed.Key == ConsoleKey.DownArrow)
            {
                selectedMenuElement++;
                if (selectedMenuElement > menus.Count - 1)
                    selectedMenuElement = menus.Count - 1;
            }

            Console.BackgroundColor = backgroundColor;
        }
    }

    private void RefreshConsoleWindow(bool drawFrame = true)
    {
        Console.BackgroundColor = backgroundColor;
        Console.ForegroundColor = foregroundColor;

        Console.Write("\x1b[3J");
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        Console.Clear();

        if (drawFrame)
            DrawFrame(0, 0, Console.WindowWidth, Console.WindowHeight);
    }

    private void DrawUpperFrame()
    {
        for (int i = 0; i < Console.BufferWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("═");
        }
        Console.SetCursorPosition(0, 0);
        Console.Write("╔");
        Console.SetCursorPosition(Console.BufferWidth-1, 0);
        Console.Write("╗");
    }

    private void DrawBottomFrame()
    {
        for (int i = 0; i < Console.BufferWidth; i++)
        {
            Console.SetCursorPosition(i, Console.BufferHeight-1);
            Console.Write("═");
        }
        Console.SetCursorPosition(Console.BufferWidth-1, Console.BufferHeight-1);
        Console.Write("╝");
        Console.SetCursorPosition(0, Console.BufferHeight - 1);
        Console.Write("╚");
    }

    private void DrawFrame(int left, int top, int width, int height)
    {
        if (left <= 0)
            left = 0;
        if (top <= 0)
            top = 0;
        if (width >= Console.BufferWidth)
            width = Console.BufferWidth - 1;
        if (height >= Console.BufferHeight)
            height = Console.BufferHeight - 1;

        for (int i = left; i < left + width; i++)
        {
            Console.SetCursorPosition(i, top);
            Console.Write("═");
            Console.SetCursorPosition(i, top + height);
            Console.Write("═");
        }

        for (int j = top; j < top + height; j++)
        {
            Console.SetCursorPosition(left, j);
            Console.Write("║");
            Console.SetCursorPosition(left + width, j);
            Console.Write("║");
        }

        Console.SetCursorPosition(left, top);
        Console.Write("╔");
        Console.SetCursorPosition(left, top + height);
        Console.Write("╚");
        Console.SetCursorPosition(left + width, top);
        Console.Write("╗");
        Console.SetCursorPosition(left + width, top + height);
        Console.Write("╝");
        GetAppHeader();
    }

    private void GetAppHeader()
    {
        string[] title = new[]
        {
            @"  _     ___ ____  ____    _    ______   __    _    ____  ____  ",
            @" | |   |_ _| __ )|  _ \  / \  |  _ \ \ / /   / \  |  _ \|  _ \ ",
            @" | |    | ||  _ \| |_) |/ _ \ | |_) \ V /   / _ \ | |_) | |_) |",
            @" | |___ | || |_) |  _ </ ___ \|  _ < | |   / ___ \|  __/|  __/ ",
            @" |_____|___|____/|_| \_/_/  \_\_| \_\|_|  /_/   \_\_|   |_|   ",
        };

        int startX = Console.BufferWidth / 2 - title[0].Length / 2;
        int startY = 1;

        for (int i = 0; i < title.Length; i++)
        {
            Console.SetCursorPosition(startX, startY + i);
            Console.Write(title[i]);
        }
    }
}