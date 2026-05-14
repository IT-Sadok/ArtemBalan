namespace LibraryApp;

public interface IFileManager
{
    public  List<T> Load<T>();
    public void Save<T>(List<T> data);
}