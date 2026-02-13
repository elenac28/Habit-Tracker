using HabitTracker.Models;

namespace HabitTracker.Services;

public class HabitService
{
    private readonly StorageService _storage;
    private readonly List<Habit> _habits;

    public HabitService()
    {
        _storage = new StorageService();
        _habits = _storage.LoadHabits();

        foreach (var h in _habits)
        {
            h.Streak = CalculateDailyStreak(h.CompletedDates);
        }
    }


    public void AddHabit()
    {
        Console.Write("Enter habit name: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name.");
            return;
        }

        _habits.Add(new Habit { Name = name.Trim() });
        _storage.SaveHabits(_habits);

        Console.WriteLine("Habit added!");
        
    }

    public void ViewHabits()
    {
        
        Console.WriteLine();

        if (_habits.Count == 0)
        {
            Console.WriteLine("No habits found.");
            return;
        }

        for (int i = 0; i < _habits.Count; i++)
        {
            var h = _habits[i];
            h.Streak = CalculateDailyStreak(h.CompletedDates);

            Console.WriteLine($"{i + 1}. {h.Name} (Streak: {h.Streak}, Completed: {h.CompletedDates.Count})");
        }
    }

    public void MarkHabitComplete()
    {
        if (_habits.Count == 0)
        {
            Console.WriteLine("No habits found.");
            return;
        }

        ViewHabits();
        Console.Write("Select habit number: ");

        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        index--;

        if (index < 0 || index >= _habits.Count)
        {
            Console.WriteLine("Habit not found.");
            return;
        }

        var habit = _habits[index];

        if (!habit.CompletedDates.Contains(DateTime.Today))
        {
            habit.CompletedDates.Add(DateTime.Today);
            habit.Streak = CalculateDailyStreak(habit.CompletedDates);
            _storage.SaveHabits(_habits);
            Console.WriteLine("Marked complete!");
        }
        else
        {
            Console.WriteLine("Already completed today.");
        }
    }

    public void EditHabitName()
{
    if (_habits.Count == 0)
    {
        Console.WriteLine("No habits found.");
        return;
    }

    ViewHabits();
    Console.Write("Select habit number to edit: ");

    if (!int.TryParse(Console.ReadLine(), out int index))
    {
        Console.WriteLine("Invalid number.");
        return;
    }

    index--;

    if (index < 0 || index >= _habits.Count)
    {
        Console.WriteLine("Habit not found.");
        return;
    }

    Console.Write("Enter new habit name: ");
    var newName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(newName))
    {
        Console.WriteLine("Invalid name.");
        return;
    }

    _habits[index].Name = newName.Trim();
    _storage.SaveHabits(_habits);

    Console.WriteLine("Habit updated!");
}
    public void DeleteHabit()
    {
        if (_habits.Count == 0)
        {
            Console.WriteLine("No habits found.");
            return;
        }

        ViewHabits();
        Console.Write("Select habit number to delete: ");

        if (!int.TryParse(Console.ReadLine(), out int index))
        {
            Console.WriteLine("Invalid number.");
            return;
        }

        index--;

        if (index < 0 || index >= _habits.Count)
        {
            Console.WriteLine("Habit not found.");
            return;
        }

        Console.Write($"Delete \"{_habits[index].Name}\"? (y/n): ");
        var confirm = Console.ReadLine();

        if (confirm?.Trim().ToLower() != "y")
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        _habits.RemoveAt(index);
        _storage.SaveHabits(_habits);

        Console.WriteLine("Habit deleted!");
    }

    private static int CalculateDailyStreak(List<DateTime> completedDates)
{
    if (completedDates == null || completedDates.Count == 0)
        return 0;

    // Normalize to dates only, remove duplicates, sort newest->oldest
    var dates = completedDates
        .Select(d => d.Date)
        .Distinct()
        .OrderByDescending(d => d)
        .ToList();

    int streak = 0;
    var day = DateTime.Today;

    // Count consecutive days ending today
    while (dates.Contains(day))
    {
        streak++;
        day = day.AddDays(-1);
    }

    return streak;
}


}
