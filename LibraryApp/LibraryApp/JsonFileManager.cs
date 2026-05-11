using System.Text.Json;

namespace LibraryApp;

public static class JsonFileManager
{
    private static string FilePath = "books.json";

    
    public static List<T> Load<T>()
    {
        if (!File.Exists(FilePath))
            return new List<T>();

        var json = File.ReadAllText(FilePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<T>();

        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

    }

    public static void Save<T>(List<T> data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(FilePath, json);
    }
}