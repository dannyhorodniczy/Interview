using System;
using System.IO;

namespace WebApi;

public class FileManager
{
    private readonly string _filePath;

    public FileManager(string baseFilePath = "C:\\Dev\\database.txt")
    {
        _filePath = baseFilePath;
    }

    public void WriteToFile(string content)
    {
        try
        {
            File.WriteAllText(_filePath, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
        }
    }

    public string ReadFromFile()
    {
        try
        {
            return File.ReadAllText(_filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading from the file: {ex.Message}");
            return string.Empty;
        }
    }
}
