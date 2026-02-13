using System.Text.Json;
using HabitTracker.Models;

namespace HabitTracker.Services;

public class StorageService
{
    private readonly string _filePath;

    public StorageService(string appFolderName = "HabitTracker", string fileName = "habits.json")
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            appFolderName
        );

        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, fileName);
    }

    public string GetFilePath() => _filePath;

    public List<Habit> LoadHabits()
{
    try
    {
        if (!File.Exists(_filePath))
            return new List<Habit>();

        var json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<Habit>();

        return JsonSerializer.Deserialize<List<Habit>>(json) ?? new List<Habit>();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[LOAD ERROR] {ex.GetType().Name}: {ex.Message}");
        Console.WriteLine($"[LOAD PATH] {_filePath}");
        Console.WriteLine("Press any key...");
        Console.ReadKey();
        return new List<Habit>();
    }
}

public void SaveHabits(List<Habit> habits)
{
    try
    {
        var json = JsonSerializer.Serialize(habits, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SAVE ERROR] {ex.GetType().Name}: {ex.Message}");
        Console.WriteLine($"[SAVE PATH] {_filePath}");
        Console.WriteLine("Press any key...");
        Console.ReadKey();
        throw; // crash so we *see* it during debugging
    }
}
}
