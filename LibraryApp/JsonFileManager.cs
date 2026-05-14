using System.Text.Json;

namespace LibraryApp;



public class JsonFileManager: IFileManager
{
    private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true
    };
    private static string FilePath = "books.json";

    
    public List<T> Load<T>()
    {
        if (!File.Exists(FilePath))
            return new List<T>();

        var json = File.ReadAllText(FilePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<T>();

        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

    }

    public void Save<T>(List<T> data)
    {
        
        var json = JsonSerializer.Serialize(data, jsonSerializerOptions);
        File.WriteAllText(FilePath, json);
    }
}